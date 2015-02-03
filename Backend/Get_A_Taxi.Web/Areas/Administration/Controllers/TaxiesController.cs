using Get_A_Taxi.Data;
using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Controllers;
using Get_A_Taxi.Web.Infrastructure;
using Get_A_Taxi.Web.Infrastructure.Services.Contracts;
using Get_A_Taxi.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Get_A_Taxi.Web.Areas.Administration.Controllers
{
    [AuthorizeRoles(UserRoles=UserRoles.Administrator)]// | UserRoles.Manager)]
    public class TaxiesController : BaseController
    {
        private ITaxiService taxiService;
        private const int TAXI_RESULTS_DEFAULT_COUNT = 10;

        public TaxiesController(IGetATaxiData data, ITaxiService taxiService)
            :base(data)
        {
            this.taxiService = taxiService;

        }

        // Based on current user's District
        // GET: Administration/Taxies
        public ActionResult Index()
        {
           // var taxies = this.Data.Taxies.All().Select(TaxiViewModel.FromTaxiDataModel).ToList
            var district = UserProfile.District;
            var taxiesListVM = this.Data.Taxies.All()
                .Where(t => t.District.DistrictId == UserProfile.District.DistrictId)
                .Take(TAXI_RESULTS_DEFAULT_COUNT)
                .Select(TaxiItemVM.FromTaxiDataModel)
                .ToList();
            return View("Taxies", taxiesListVM);
        }

        // GET: Administration/Taxies/Details/5
        public ActionResult Details(int taxiId)
        {
            var taxiVM = this.Data.Taxies
                .SearchFor(t => t.TaxiId == taxiId)
                .Select(TaxiDetailsVM.FromTaxiDataModel)
                .FirstOrDefault();
            return PartialView("_TaxiDetailsPartialView", taxiVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchTaxi(string plate, string driver, string district)
        {
            var taxiesListVM = this.taxiService.GetByAllProps(plate, driver, district, this.RoleManager)
                .Take(TAXI_RESULTS_DEFAULT_COUNT)
                .Select(TaxiItemVM.FromTaxiDataModel)
                .ToList();
            return PartialView("_TaxiesListPartialView", taxiesListVM);
        }

        [HttpPost]
        public ActionResult SearchDriver(string driverName, string district)
        {
            var driversListVM = this.taxiService.GetDriversByNameAndDistrict(driverName, district, this.RoleManager)
                .Select(UserItemViewModel.FromApplicationUserModel)
                .ToList();
            var roleItems = this.GetRolesSelectList();
            ViewBag.UserRoles = roleItems;
            return PartialView("_UsersListPartialView", driversListVM);
        }

        [HttpGet]
        // GET: Administration/Taxies/UserDetails/{string(guid)}
        public ActionResult UserDetails(string userId)
        {
            var accountInfoVM = this.Data.Users.All()
               .Where(u => u.Id == userId)
               .Select(UserInfoVM.FromApplicationUserModel)
               .FirstOrDefault();

            return PartialView("_UserInfoPartialView", accountInfoVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignDriver(string userId, int taxiId)
        {
            var driverRoleId = this.RoleManager.Roles
                .Where(r => r.Name == UserRoles.Driver.ToString())
                .FirstOrDefault().Id;

            var taxi = this.Data.Taxies.All()
                .Where(t => t.TaxiId == taxiId && t.Driver == null)
                .FirstOrDefault();

            var driver = this.Data.Users.All()
                .FirstOrDefault(u => u.Id == userId);

            if (!taxi.Available)
            {
                TempData["Error"] = "Taxi not available!";
                return RedirectToAction("Index");
            }

            if (!driver.Roles.Select(d => d.RoleId).Contains(driverRoleId))
            {
                TempData["Error"] = "Driver not found!";
                return RedirectToAction("Index");
            }

            taxi.Driver = driver;
            this.Data.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnassignDriver(string userId, int taxiId)
        {
            var taxi = this.Data.Taxies.All()
                .Where(t => t.TaxiId == taxiId && t.Driver.Id == userId)
                .FirstOrDefault();
            if (taxi != null && taxi.Driver != null)
            {
                
                taxi.Driver = null;
                this.Data.SaveChanges();
            }
            else
            {
                TempData["Error"] = "Taxi with this driver was not found!";
            }
            return RedirectToAction("Index");
        }

        // GET: Administration/Taxies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Administration/Taxies/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Administration/Taxies/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Administration/Taxies/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Administration/Taxies/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Administration/Taxies/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
