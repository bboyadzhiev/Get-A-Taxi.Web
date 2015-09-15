using Get_A_Taxi.Data;
using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Areas.Operator.ViewModels;
using Get_A_Taxi.Web.Infrastructure.Authorization;
using Get_A_Taxi.Web.Infrastructure.Populators;
using Get_A_Taxi.Web.Infrastructure.Services.Contracts;
using Get_A_Taxi.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using Get_A_Taxi.Web.Infrastructure.Bridges;
using Get_A_Taxi.Web.Models;
using Newtonsoft.Json;

namespace Get_A_Taxi.Web.Areas.Operator.Controllers
{
    [AuthorizeRoles(UserRole = UserRoles.Operator)]
    public class MainController : BaseController
    {
        private IAccountService services;
        private IOrderBridge bridge;
        public MainController(IGetATaxiData data, IAccountService services, IDropDownListPopulator populator, IOrderBridge bridge)
            : base(data, populator)
        {
            this.services = services;
            this.bridge = bridge;
        }
        // GET: Operator/Main
        public ActionResult Index()
        {
            ViewBag.Lat = this.UserProfile.District.CenterLatitude;
            ViewBag.Lng = this.UserProfile.District.CenterLongitude;
            ViewBag.MapZoom = this.UserProfile.District.MapZoom;
            ViewBag.DistrictId = this.UserProfile.District.DistrictId;
            ViewBag.OperatorId = this.UserProfile.Id;
            return View();
        }

       // [AllowAnonymous]
        [HttpGet]
        public JsonResult GetTaxiStands()
        {
            var result = this.Data.Stands.All()
                .Where(s => s.District.DistrictId == this.UserProfile.District.DistrictId)
                .ToList()
                .Select(x => new 
                { 
                    lat = x.Latitude, 
                    lon = x.Longitude,
                    alias = x.Alias
                });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Create a phone order, added by the operator.
        /// OperatorOrders stores the current orders added by the operators.
        /// If client phone is not found in the database, a new user account is created using the new phone number.
        /// Last order with same orderId and operator ID and the user comment are updated in OperatorOrders.
        /// </summary>
        /// <param name="orderVm">Input ViewModel of a phone order</param>
        /// <returns>Partial view with phone order input form</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public async Task<ActionResult> CreateOrder([Bind(Include = "OrderLatitude,OrderLongitude,OrderAddress,DestinationLatitude,DestinationLongitude,DestinationAddress,FirstName,LastName,PhoneNumber,UserComment")]OrderInputVM orderVm)
        {
            if (ModelState.IsValid)
            {
                ModelState.Clear();
                Order order;
                var operatorUser = this.Data.Users.SearchFor(u => u.Id == UserProfile.Id).FirstOrDefault();
                var district = operatorUser.District;
                var knownClient = this.Data.Users.SearchFor(u => u.PhoneNumber == orderVm.PhoneNumber).FirstOrDefault();
                if (knownClient != null)
                {
                    // Known client, new order
                    order = OrderInputVM.ToOrderDataModel(orderVm, knownClient);
                    order.OrderStatus = OrderStatus.Unassigned;
                    order.District = district;
                    this.Data.Orders.Add(order);
                    this.Data.Orders.SaveChanges();
                }
                else
                {
                    //New client
                    // TODO: Review phone user creation!
                    ApplicationUser newClient = new ApplicationUser()
                    {
                        UserName = orderVm.PhoneNumber + "@getataxi.com",
                        PhoneNumber = orderVm.PhoneNumber,

                        // TODO: Phone user default address
                        DefaultAddress = orderVm.OrderAddress,
                        FirstName = orderVm.FirstName,
                        LastName = orderVm.LastName,
                        Email = orderVm.PhoneNumber + "@getataxi.com"
                    };

                    var id = newClient.Id;
                    var password = this.services.CreatePassword(15);
                    var result = await UserManager.CreateAsync(newClient, password);
                    if (result.Succeeded)
                    {
                        //New order for the new client
                        order = OrderInputVM.ToOrderDataModel(orderVm, newClient);
                        order.OrderStatus = OrderStatus.Unassigned;
                        order.District = district;
                        this.Data.Orders.Add(order);
                        this.Data.Orders.SaveChanges();
                    }
                    else
                    {
                        ViewBag.Error = "Order could not be added:" + result.Errors.ToString();
                        return PartialView("_OrderInputPartialView", orderVm);
                    }
                }

                // New order by operator
                var operatorOrder = new OperatorOrder()
                {
                    OperatorId = operatorUser.Id,
                    OrderId = order.OrderId,
                    Comment = order.UserComment
                };
                this.Data.OperatorsOrders.Add(operatorOrder);
                this.Data.OperatorsOrders.SaveChanges();
                var orderVM = this.Data.Orders.All().Where(o => o.OrderId == order.OrderId).Project().To<OrderDetailsDTO>().FirstOrDefault();
                this.bridge.AddOrder(orderVM, district.DistrictId);
                //this.bridge.AddOrder(order.OrderId, district.DistrictId);

                return PartialView("_OrderInputPartialView", new OrderInputVM());
            }
            return PartialView("_OrderInputPartialView", orderVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult UpdateOrder(OrderInputVM orderVm)
        {
            if (ModelState.IsValid)
            {
                var order = this.Data.Orders.SearchFor(o => o.OrderId == orderVm.OrderId).FirstOrDefault();
                if (order != null)
                {
                    // HACK: (EF hack) re-assigning district and customer
                    var operatorUser = this.Data.Users.SearchFor(u => u.Id == UserProfile.Id).FirstOrDefault();
                    var district = operatorUser.District;
                    var customer = this.Data.Users.SearchFor(u => u.Id == order.Customer.Id).FirstOrDefault();
                    OrderInputVM.UpdateOrderFromOperator(orderVm, order);
                    order.District = district;
                    order.Customer = customer;
                    this.Data.Orders.Update(order);

                    // Updating or adding a new OperatorOrder entity
                    var lastOperatorOrder = this.Data.OperatorsOrders.All().Where(o => o.OrderId == orderVm.OrderId).FirstOrDefault();
                    if (lastOperatorOrder != null)
                    {
                        // Update operator Id and comment
                        lastOperatorOrder.OperatorId = operatorUser.Id;
                        lastOperatorOrder.Comment = orderVm.UserComment;
                        this.Data.OperatorsOrders.Update(lastOperatorOrder);
                    }
                    else
                    {
                        // Mobile client order that is updated by the operator
                        var operatorOrder = new OperatorOrder()
                        {
                            OperatorId = operatorUser.Id,
                            OrderId = orderVm.OrderId,
                            Comment = orderVm.UserComment
                        };
                        this.Data.OperatorsOrders.Add(operatorOrder);
                    }
                    this.Data.SaveChanges();
                    var orderVM = this.Data.Orders.All().Where(o => o.OrderId == order.OrderId).Project().To<OrderDetailsDTO>().FirstOrDefault();
                    this.bridge.UpdateOrder(orderVM, district.DistrictId);
                }
                else
                {
                    ViewBag.Error = "Order could not be found!";
                    return PartialView("_OrderInputPartialView", orderVm);
                }

                return PartialView("_OrderInputPartialView", new OrderInputVM());
            }
            return PartialView("_OrderInputPartialView", orderVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult CancelOrder(int cancelOrderId)
        {
            var order = this.Data.Orders.SearchFor(o => o.OrderId == cancelOrderId).FirstOrDefault();
            if (order != null)
            {
                switch (order.OrderStatus)
                {
                    case OrderStatus.Unassigned:
                        break;
                    case OrderStatus.Waiting:
                        break;
                    case OrderStatus.InProgress: return ErrorResult("Order cannot be cancelled!");
                    case OrderStatus.Finished: return ErrorResult("Order is already finished!");
                    case OrderStatus.Cancelled: return ErrorResult("Order is already cancelled!");
                    default:
                        break;
                }

                // Updating or adding a new OperatorOrder entity
                var operatorUser = this.Data.Users.SearchFor(u => u.Id == UserProfile.Id).FirstOrDefault();
                var lastOperatorOrder = this.Data.OperatorsOrders.All().Where(o => o.OrderId == cancelOrderId).FirstOrDefault();
                if (lastOperatorOrder != null)
                {
                    // Update operator Id and comment
                    lastOperatorOrder.OperatorId = operatorUser.Id;
                    lastOperatorOrder.Comment += " Cancelled at " + DateTime.Now.ToString();
                    this.Data.OperatorsOrders.Update(lastOperatorOrder);
                }
                else
                {
                    // Mobile client order that is being cancelled by the operator
                    var operatorOrder = new OperatorOrder()
                    {
                        OperatorId = operatorUser.Id,
                        OrderId = order.OrderId,
                        Comment = "Cancelled at " + DateTime.Now.ToString()
                    };
                    this.Data.OperatorsOrders.Add(operatorOrder);
                }

                order.OrderStatus = OrderStatus.Cancelled;
                var customer = order.Customer;
                var district = order.District;
                this.Data.Orders.Update(order);
                this.Data.Orders.SaveChanges();

                this.bridge.CancelOrder(order.OrderId, district.DistrictId);
            }
            else
            {
                return ErrorResult("Order could not be found!");
            }
            return PartialView("_OrderInputPartialView", new OrderInputVM());
        }

        private ActionResult ErrorResult(string error)
        {
            ViewBag.Error = error;
            return Json(new
            {
                error = error
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchUser(string phoneNumber)
        {
            var user = this.Data.Users.SearchFor(u => u.PhoneNumber == phoneNumber).FirstOrDefault();

            if (user != null)
            {
                return Json(new
                {
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    orderAddress = user.DefaultAddress
                });
            }
            return null;
        }

    }
}