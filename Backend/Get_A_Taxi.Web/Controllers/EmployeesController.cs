using AutoMapper;
using AutoMapper.QueryableExtensions;
using Get_A_Taxi.Data;
using Get_A_Taxi.Data.Migrations;
using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure;
using Get_A_Taxi.Web.Infrastructure.Populators;
using Get_A_Taxi.Web.Infrastructure.Services;
using Get_A_Taxi.Web.Infrastructure.Services.Contracts;
using Get_A_Taxi.Web.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Get_A_Taxi.Web.Controllers
{
    [AuthorizeRoles(UserRole = UserRoles.Administrator, SecondRole =  UserRoles.Manager)]
   // [AuthorizeRoles(UserWithRoles = new HashSet<UserRoles>() { UserRoles.Administrator})]
    public class EmployeesController : BaseController
    {
        private IAccountService services;

        public EmployeesController(IGetATaxiData data, IAccountService services, IDropDownListPopulator populator)
            : base(data, populator)
        {
            this.services = services;
        }


        // GET: Employees
        public ActionResult Index()
        {
            ViewBag.UserRoles = this.populator.GetRoles(this.RoleManager);
            ViewBag.DistrictsList = this.populator.GetDistricts();
            var district = UserProfile.District;
            var employees = this.Data.Users.All()
                .Where(u => u.District.DistrictId == district.DistrictId)
                .Project().To<UserItemViewModel>()
                .ToList();
            return View(employees);
        }

        // GET: Employees/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = this.Data.Users.SearchFor(u => u.Id == id);
            var userDetailsVM = user.Project().To<UserDetailsVM>().FirstOrDefault();
            if (userDetailsVM == null)
            {
                return HttpNotFound();
            }
            return View(userDetailsVM);
        }
       

        // GET: Employees/Create
        public ActionResult Create()
        {
            var newUser = new UserDetailsVM();
            newUser.UserRoles = this.populator.GetRoles(this.RoleManager);
            newUser.DistrictsList = this.populator.GetDistricts();
            return View(newUser);
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create(string[] selectedRoles, [Bind(Include = "FirstName,MiddleName,LastName,DefaultAddress,DistritId,Email,PhoneNumber,SelectedRoleIds,UserRoles")] UserDetailsVM userDetailsVM)
        public async Task<ActionResult> Create([Bind(Include = "FirstName,MiddleName,LastName,DefaultAddress,DistritId,Email,PhoneNumber,SelectedRoleIds")] UserDetailsVM userDetailsVM)
        {
            if (ModelState.IsValid)
            {

                ApplicationUser employee = new ApplicationUser() { UserName = userDetailsVM.Email };
                var id = employee.Id;
                Mapper.Map<UserDetailsVM, ApplicationUser>(userDetailsVM, employee);
                employee.Id = id;

                var password = this.services.CreatePassword(15);

                var result = await UserManager.CreateAsync(employee, password);

                if (result.Succeeded)
                {
                    UpdateUserDistrict(userDetailsVM.DistritId, id);
                    UpdateUserRoles(userDetailsVM.SelectedRoleIds, id);
                    UserManager.SendEmail(employee.Id, "Welcome, " + employee.FirstName + " " + employee.LastName, "Your password is: " + password);
                }
                return RedirectToAction("Index");
            }
            return View(userDetailsVM);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = this.Data.Users.SearchFor(u => u.Id == id);
            if (user.FirstOrDefault() == null)
            {
                return HttpNotFound();
            }

            var userDetailsVM = user.Project().To<UserDetailsVM>().FirstOrDefault();
            List<string> userRoles = user.FirstOrDefault().Roles.AsQueryable().Select(r => r.RoleId).ToList();
            IEnumerable<SelectListItem> roleItems;

            if (User.IsInRole(UserRoles.Administrator.ToString()))
            {
                roleItems = this.populator.GetRoles(this.RoleManager);
            }
            else
            {
                roleItems = this.populator.GetRolesForManagement(this.RoleManager);
            }

            // Make role marked if the user is already in it
            foreach (var item in roleItems)
            {
                item.Selected = userRoles.Contains(item.Value);
            }
            userDetailsVM.UserRoles = roleItems;

            userDetailsVM.DistrictsList = this.populator.GetDistricts();
            return View(userDetailsVM);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,MiddleName,LastName,DefaultAddress,DistritId,Email,PhoneNumber,SelectedRoleIds")] UserDetailsVM userDetailsVM)
        {
            if (ModelState.IsValid)
            {
                var id = userDetailsVM.Id;
                var employee = UserManager.FindById(id);
                Mapper.Map<UserDetailsVM, ApplicationUser>(userDetailsVM, employee);
                employee.Id = id;

                employee.Roles.Clear();
                UpdateUserRoles(userDetailsVM.SelectedRoleIds, id);
                UpdateUserDistrict(userDetailsVM.DistritId, id);

                return RedirectToAction("Index");
            }
            return View(userDetailsVM);
        }

        // GET: Employees/Delete/5
        [AuthorizeRoles(UserRole = UserRoles.Administrator)]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = this.Data.Users.SearchFor(u => u.Id == id);
            if (user.FirstOrDefault() == null)
            {
                return HttpNotFound();
            }

            var userDetailsVM = user.Project().To<UserDetailsVM>().FirstOrDefault();
            return View(userDetailsVM);
        }

        // Just cleans the user roles
        // POST: Employees/Delete/5
        [AuthorizeRoles(UserRole = UserRoles.Administrator)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var user = this.Data.Users.SearchFor(u => u.Id == id).FirstOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }

            user.Roles.Clear();
            this.Data.SaveChanges();

            return RedirectToAction("Index");
        }

        //public ActionResult Search(string query)
        //{
        //    var accountsVM = this.services.GetAccountsByTextSearch(query);
        //    //var roleItems = this.GetRolesSelectList();
        //    var roleItems = this.populator.GetRoles(this.RoleManager);
        //    ViewBag.UserRoles = roleItems;
        //    return this.PartialView("_UsersListPartialView", accountsVM);
        //}



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
                    var district = this.Data.Districts.SearchFor(d=>d.DistrictId == userSearchVM.DistritId).FirstOrDefault();
                    result = this.services.WithDistrictLike(result, district.Title);
                }
            }

            var accountsVM = result.Project().To<UserItemViewModel>()
                .ToList();
            var roleItems = this.populator.GetRoles(this.RoleManager);
            ViewBag.UserRoles = roleItems;
            return this.PartialView("_UsersListPartialView", accountsVM);
        }

        #region Helpers

        private void UpdateUserDistrict(int districtId, string userId)
        {
            var user = this.Data.Users.SearchFor(u => u.Id == userId).FirstOrDefault();
            //var user = this.UserManager.FindById(employee.Id);
            var district = this.Data.Districts.SearchFor(d => d.DistrictId == districtId).FirstOrDefault();
            // if (user.District.DistrictId != district.DistrictId)
            // {
            user.District = district;
            this.Data.SaveChanges();
            // }

        }

        private void UpdateUserRoles(string[] roleIds, string userId)
        {
            // var user = this.UserManager.FindById(userId);
            //user.Roles.Clear();
            if (User.IsInRole(UserRoles.Administrator.ToString()))
            {
                foreach (string roleId in roleIds)
                {
                    var roleName = this.RoleManager.FindById(roleId).Name;
                    this.UserManager.AddToRole(userId, roleName);
                }
            }
            else
            {
                foreach (string roleId in roleIds)
                {
                    var roleName = this.RoleManager.FindById(roleId).Name;
                    if (roleName == UserRoles.Driver.ToString() || roleName == UserRoles.Operator.ToString())
                    {
                        this.UserManager.AddToRole(userId, roleName);
                    }
                }
            }
        }

        #endregion
    }
}
