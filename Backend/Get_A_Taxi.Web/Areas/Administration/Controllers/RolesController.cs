using Get_A_Taxi.Data;
using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Areas.Administration.ViewModels;
using Get_A_Taxi.Web.Infrastructure.Authorization;
using Get_A_Taxi.Web.Infrastructure.Populators;
using Get_A_Taxi.Web.Infrastructure.Services;
using Get_A_Taxi.Web.Infrastructure.Services.Contracts;
using Get_A_Taxi.Web.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;

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
    [AuthorizeRoles(UserRole = UserRoles.Administrator)]
    public class RolesController : BaseController
    {

        private IAccountService services;
        public RolesController(IGetATaxiData data, IDropDownListPopulator populator)
            : base(data, populator)
        {
            this.services = new AccountService(data);
        }
        // GET: Administration/Roles
        public ActionResult Index()
        {
            var rolesViewModel = new RolesAdministrationVM();

            rolesViewModel.Accounts = this.services.AllUsers().Project().To<UserItemViewModel>()
                .ToList();

            ViewBag.UserRoles = this.populator.GetRoles(this.RoleManager);
            ViewBag.DistrictsList = this.populator.GetNullableDistricts();
            return View("Roles",rolesViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search([Bind(Include = "FirstName,MiddleName,LastName,DistritId,SelectedRoleIds")] UserSearchVM userSearchVM)
        {
            var result = this.services.AllUsers();
            if (ModelState.IsValid)
            {
                if (userSearchVM.FirstName != null)
                {
                    result = this.services.WithFirstNameLike(result, userSearchVM.FirstName);
                }
                if (userSearchVM.MiddleName != null)
                {
                    result = this.services.WithMiddletNameLike(result, userSearchVM.MiddleName);
                }
                if (userSearchVM.LastName != null)
                {
                    result = this.services.WithLastNameLike(result, userSearchVM.LastName);
                }
                if (userSearchVM.SelectedRoleIds != null)
                {
                    foreach (var role in userSearchVM.SelectedRoleIds)
                    {
                        result = this.services.WithRole(result, role);
                    }
                }
                if (userSearchVM.DistritId != 0)
                {
                    var district = this.Data.Districts.SearchFor(d => d.DistrictId == userSearchVM.DistritId).FirstOrDefault();
                    result = this.services.WithDistrictLike(result, district.Title);
                }
            }

            var accountsVM = result.Project().To<UserItemViewModel>()
                .ToList();
            var roleItems = this.populator.GetRoles(this.RoleManager);
            ViewBag.UserRoles = roleItems;
            return this.PartialView("_UsersListPartialView", accountsVM);
        }

        [HttpGet]
        // GET: Administration/Roles/Details/{string}
        public ActionResult Details(string id)
        {
            var user = this.Data.Users.All().First(u => u.Id == id);
            List<string> userRoles = user.Roles.AsQueryable().Select(r => r.RoleId).ToList();
            var roleItems = this.populator.GetRoles(this.RoleManager);

            foreach (var item in roleItems)
            {
                item.Selected = userRoles.Contains(item.Value);
            }

            var accountVM = this.Data.Users.All()
                .Where(u => u.Id == id)
                .Select(RolesEditVM.FromApplicationUserModel)
                .FirstOrDefault();

            accountVM.UserRoles = roleItems;
            return PartialView("_RolesEditPartialView", accountVM);
        }

        [HttpPost]
        [AuthorizeRoles(UserRole = UserRoles.Administrator)]
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
            if (selectedRoles != null && selectedRoles.Length != 0)
            {
                foreach (string role in selectedRoles)
                {
                    this.UserManager.AddToRole(userId.ToString(), role);
                }
            } 
            this.Data.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
