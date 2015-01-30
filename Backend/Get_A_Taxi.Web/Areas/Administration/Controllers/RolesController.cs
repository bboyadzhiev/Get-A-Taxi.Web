using Get_A_Taxi.Data;
using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Controllers;
using Get_A_Taxi.Web.Infrastructure.Services.Contracts;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Get_A_Taxi.Web.Areas.Administration.ViewModels; // Maybe this one too

namespace Get_A_Taxi.Web.Areas.Administration.Controllers
{
    public static class MyExtensions
    {
        public static SelectList ToSelectList<TEnum>(this TEnum enumObj)
            where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                         select new { Id = e, Name = e.ToString() };
            return new SelectList(values, "Id", "Name", enumObj);
        }
    }
    [Authorize(Roles = "Administrator")]
    public class RolesController : BaseController
    {

        private IAccountService services;
        public RolesController(IGetATaxiData data, IAccountService services)
            : base(data)
        {
            this.services = services;
        }
        // GET: Administration/Roles
        public ActionResult Index()
        {

            var rolesViewModel = new RolesAdministrationVM();

            rolesViewModel.Accounts = this.services.GetAccounts();

            var roleItems = this.GetRolesSelectList();
            ViewBag.UserRoles = roleItems;
            return View("Roles",rolesViewModel);
           
        }

        


        public ActionResult Search(string query)
        {
            var accountsVM = this.services.GetAccountsByTextSearch(query);
            var roleItems = this.GetRolesSelectList();
            ViewBag.UserRoles = roleItems;
            return this.PartialView("_UsersListPartialView", accountsVM);
        }

        [HttpGet]
        // GET: Administration/Roles/Details/5
        public ActionResult Details(string userId)
        {
            var user = this.Data.Users.All().First(u => u.Id == userId);
            List<string> userRoles = user.Roles.AsQueryable().Select(r => r.RoleId).ToList();

            var roles = RoleManager.Roles.ToList();
            List<SelectListItem> roleItems = new List<SelectListItem>();
            foreach (var role in roles)
            {
                roleItems.Add(new SelectListItem
                {
                    Text = role.Name,
                    Value = role.Id,
                    Selected = userRoles.Contains(role.Id)
                });
            }
            var accountVM = this.Data.Users.All()
                .Where(u => u.Id == userId)
                .Select(RolesEditVM.FromApplicationUserModel)
                .FirstOrDefault();

            accountVM.UserRoles = roleItems;
            return View("_AccountEditPartialView", accountVM);
        }

        //public ActionResult Details(string userId)
        //{
        //    var user = this.Data.Users.All().First(u => u.Id == userId);
        //    List<string> userRoles = user.Roles.AsQueryable().Select(r => r.RoleId).ToList();

        //    var roles = RoleManager.Roles.ToList();
        //    List<SelectListItem> roleItems = new List<SelectListItem>();
        //    foreach (var role in roles)
        //    {
        //        roleItems.Add(new SelectListItem
        //        {
        //            Text = role.Name,
        //            Value = role.Id,
        //            Selected = userRoles.Contains(role.Id)
        //        });
        //    }
        //    var accountVM = this.Data.Users.All()
        //        .Where(u => u.Id == userId)
        //        .Select(AccountEditVM.FromApplicationUserModel)
        //        .FirstOrDefault();

        //    // accountVM.UserRoles = userRoles;
        //    return View("_AccountEditPartialView", accountVM);
        //}

        [HttpPost]
        [Authorize(Roles="Administrator")]
        [ValidateAntiForgeryToken]
        public ActionResult Update(string[] selectedRoles, Guid userId)
        {
            var currentUserId = User.Identity.GetUserId();
            if (userId.ToString() == currentUserId)
            {
                TempData["Error"] = "You cannot change your role!";
                return RedirectToAction("Index");
            }

            var user = this.Data.Users.Find(userId.ToString());
            user.Roles.Clear();
            this.Data.SaveChanges();

            foreach (string role in selectedRoles)
            {
                this.UserManager.AddToRole(userId.ToString(), role);
            }
            this.Data.SaveChanges();

            return RedirectToAction("Index");
           //return View("Roles");
        }
    }
}
