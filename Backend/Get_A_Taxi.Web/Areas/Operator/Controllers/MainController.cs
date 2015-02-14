using Get_A_Taxi.Data;
using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Areas.Operator.ViewModels;
using Get_A_Taxi.Web.Controllers;
using Get_A_Taxi.Web.Infrastructure;
using Get_A_Taxi.Web.Infrastructure.Populators;
using Get_A_Taxi.Web.Infrastructure.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Get_A_Taxi.Web.Areas.Operator.Controllers
{
    [AuthorizeRoles(UserRole=UserRoles.Operator)]
    public class MainController : BaseController
    {
        private IAccountService services;
        public MainController(IGetATaxiData data, IAccountService services, IDropDownListPopulator populator)
            : base(data, populator)
        {
            this.services = services;
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
        public ActionResult CreateOrder(OrderInputVM order)
        {
            if (ModelState.IsValid)
            {
                ModelState.Clear();
                return PartialView("_AddOrderPartialView", new OrderInputVM());
            }

            return PartialView("_AddOrderPartialView", order);
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