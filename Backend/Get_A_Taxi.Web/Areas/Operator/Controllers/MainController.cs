using Get_A_Taxi.Data;
using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Areas.Operator.ViewModels;
using Get_A_Taxi.Web.Controllers;
using Get_A_Taxi.Web.Infrastructure;
using Get_A_Taxi.Web.Infrastructure.Populators;
using Get_A_Taxi.Web.Infrastructure.Services.Contracts;
using Get_A_Taxi.Web.Infrastructure.Services.Hubs;
using Get_A_Taxi.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;

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
            ViewBag.Lat = this.UserProfile.District.CenterLattitude;
            ViewBag.Lng = this.UserProfile.District.CenterLongitude;
            return View();
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
        public async Task<ActionResult> CreateOrder([Bind(Include = "OrderLattitude,OrderLongitude,OrderAddress,DestinationLattitude,DestinationLongitude,DestinationAddress,FirstName,LastName,PhoneNumber,UserComment")]OrderInputVM orderVm)
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
                        DefaultAddress = orderVm.OrderAddress,
                        FirstName = orderVm.FirstName,
                        LastName = orderVm.LastName,
                        // TODO: Review District!
                        // District = operatorUser.District,
                        Email = orderVm.PhoneNumber + "@getataxi.com"
                    };

                    var id = newClient.Id;
                    var password = this.services.CreatePassword(15);
                    var result = await UserManager.CreateAsync(newClient, password);
                    if (result.Succeeded)
                    {
                        //New order for the new client
                        order = OrderInputVM.ToOrderDataModel(orderVm, newClient);
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
                // }
                this.Data.OperatorsOrders.SaveChanges();

                this.bridge.AddOrder(order.OrderId);
                //OrdersEvents.AddOrder(order.OrderId);

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
                    var operatorUser = this.Data.Users.SearchFor(u => u.Id == UserProfile.Id).FirstOrDefault();

                    OrderInputVM.UpdateOrderFromOperator(orderVm, order);
                    this.Data.Orders.SaveChanges();

                    // Updating or adding a new OperatorOrder entity
                    var lastOperatorOrder = this.Data.OperatorsOrders.All().Where(o => o.OrderId == orderVm.OrderId).FirstOrDefault();
                    if (lastOperatorOrder != null)
                    {
                        // Update operator Id and comment
                        lastOperatorOrder.OperatorId = operatorUser.Id;
                        lastOperatorOrder.Comment = orderVm.UserComment;
                    }
                    else
                    {
                        // Mobile order, updated by the operator
                        var operatorOrder = new OperatorOrder()
                        {
                            OperatorId = operatorUser.Id,
                            OrderId = orderVm.OrderId,
                            Comment = orderVm.UserComment
                        };
                        this.Data.OperatorsOrders.Add(operatorOrder);
                    }
                    this.Data.OperatorsOrders.SaveChanges();
                    this.bridge.UpdateOrder(order.OrderId);
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
                if (order.OrderStatus == OrderStatus.Finished)
                {
                    var error = "Order could not be found!";
                    ViewBag.Error = error;
                    return Json(new
                    {
                        error = error
                    });
                }
              
                order.OrderStatus = OrderStatus.Cancelled;
                this.Data.SaveChanges();
                this.bridge.CancelOrder(order.OrderId);

                // TODO: review
                return Json(new
                {
                    success = "ok"
                });
            }
            else
            {
                var error = "Order is already finished!";
                ViewBag.Error = error;
                return Json(new
                {
                    error = error
                });
            }
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