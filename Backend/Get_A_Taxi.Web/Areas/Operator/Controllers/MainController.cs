using Get_A_Taxi.Data;
using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Areas.Operator.ViewModels;
using Get_A_Taxi.Web.Controllers;
using Get_A_Taxi.Web.Infrastructure;
using Get_A_Taxi.Web.Infrastructure.Populators;
using Get_A_Taxi.Web.Infrastructure.Services.Contracts;
using Get_A_Taxi.Web.Infrastructure.Services.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Get_A_Taxi.Web.Areas.Operator.Controllers
{
    [AuthorizeRoles(UserRole=UserRoles.Operator)]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public async Task<ActionResult> CreateOrder(OrderInputVM orderVm)
        {
            if (ModelState.IsValid)
            {
                ModelState.Clear();
                Order order;
                var operatorUser = this.Data.Users.SearchFor(u => u.Id == UserProfile.Id).FirstOrDefault();
                var knownClient = this.Data.Users.SearchFor(u => u.PhoneNumber == orderVm.PhoneNumber).FirstOrDefault();
                if (knownClient != null)
                {
                    order = OrderInputVM.ToOrderDataModel(orderVm, knownClient);
                }
                else
                {
                    ApplicationUser newClient = new ApplicationUser()
                    {
                        UserName = orderVm.PhoneNumber + "@getataxi.com",
                        PhoneNumber = orderVm.PhoneNumber,
                        DefaultAddress = orderVm.OrderAddress,
                        FirstName = orderVm.FirstName,
                        LastName = orderVm.LastName,
                     //   District = operatorUser.District,
                        Email = orderVm.PhoneNumber + "@getataxi.com"
                    };

                    var id = newClient.Id;
                    var password = this.services.CreatePassword(15);
                    var result = await UserManager.CreateAsync(newClient, password);
                    if (result.Succeeded)
                    {
                        order = OrderInputVM.ToOrderDataModel(orderVm, newClient);
                        this.Data.Orders.Add(order);
                        this.Data.SaveChanges();
                    }
                    else
                    {
                        ViewBag.Error = "Order could not be added:"+result.Errors.ToString();
                        return PartialView("_AddOrderPartialView", orderVm);
                    }
                }

                
                var operatorOrder = new OperatorOrder()
                {
                    OperatorId = operatorUser.Id,
                    OrderId = order.OrderId,
                    Comment = order.UserComment
                };
                this.Data.OperatorsOrders.Add(operatorOrder);
                this.Data.SaveChanges();

                this.bridge.AddOrder(order.OrderId);

                return PartialView("_AddOrderPartialView", new OrderInputVM());
            }
            return PartialView("_AddOrderPartialView", orderVm);
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