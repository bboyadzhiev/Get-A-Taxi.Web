using Get_A_Taxi.Data;
using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Controllers;
using Get_A_Taxi.Web.Infrastructure;
using Get_A_Taxi.Web.Infrastructure.Populators;
using Get_A_Taxi.Web.Infrastructure.Services.Contracts;
using Get_A_Taxi.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Get_A_Taxi.Web.Areas.Management.Controllers
{
    [AuthorizeRoles(UserRole = UserRoles.Administrator)]
    public class TaxiesController : BaseController
    {
        private ITaxiService taxiService;
        private const int TAXI_RESULTS_DEFAULT_COUNT = 10;

        public TaxiesController(IGetATaxiData data, ITaxiService taxiService, IDropDownListPopulator populator)
            : base(data, populator)
        {
            this.taxiService = taxiService;

        }

        // Based on current user's District
        // GET: Management/Taxies
        public ActionResult Index()
        {
            // var taxies = this.Data.Taxies.All().Select(TaxiViewModel.FromTaxiDataModel).ToList
            var district = UserProfile.District;
            var taxiesListVM = this.Data.Taxies.All()
                .Where(t => t.District.DistrictId == UserProfile.District.DistrictId)
                .Take(TAXI_RESULTS_DEFAULT_COUNT)
                .Project().To<TaxiItemVM>()
                //.Select(TaxiVM.FromTaxiDataModel)
                .ToList();
            return View("Taxies", taxiesListVM);
        }

        // GET: Management/Taxies/Details/5
        [HttpGet]
        public ActionResult Details(int taxiId)
        {
            //var taxiVM = this.Data.Taxies
            //   .All().Where(t => t.TaxiId == taxiId)
            //    .Select(TaxiDetailsVM.FromTaxiDataModel)
            //    .FirstOrDefault();
            var taxiVM = this.Data.Taxies.All().Where(t => t.TaxiId == taxiId).AsQueryable().Project().To<TaxiDetailsVM>().FirstOrDefault();
            return PartialView("_TaxiDetailsPartialView", taxiVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchTaxi(string plate, string driver, string district)
        {
            var taxiesListVM = this.taxiService.GetByAllProps(plate, driver, district, this.RoleManager)
                .Take(TAXI_RESULTS_DEFAULT_COUNT)
                //.Select(TaxiVM.FromTaxiDataModel)
                .Project().To<TaxiItemVM>()
                .ToList();
            return PartialView("_TaxiesListPartialView", taxiesListVM);
        }

        [HttpPost]
        public ActionResult SearchDriver(string driverName, string district)
        {
            var driversListVM = this.taxiService.GetDriversByNameAndDistrict(driverName, district, this.RoleManager)
               .Project().To<UserItemViewModel>()
                //.Select(UserItemViewModel.FromApplicationUserModel)
                .ToList();
            //var roleItems = this.GetRolesSelectList();
            var roleItems = this.populator.GetRoles(this.RoleManager);
            ViewBag.UserRoles = roleItems;
            return PartialView("_UsersListPartialView", driversListVM);
        }

        // GET: Management/Taxies/UserDetails/{string(guid)}
        [HttpGet]
        public ActionResult UserDetails(string userId)
        {
            var accountInfoVM = this.Data.Users.All()
               .Where(u => u.Id == userId)
               .Project().To<UserDetailsVM>()
                //.Select(UserDetailsVM.FromApplicationUserModel)
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

        // GET: Management/Taxies/Create
        [HttpGet]
        public ActionResult Create()
        {
            var districts = this.populator.GetDistricts();
            ViewBag.Districts = districts;
            return View();
        }

        // POST: Management/Taxies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TaxiDetailsVM newTaxi)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    District district = this.Data.Districts.All().First(d => d.DistrictId == newTaxi.AssignDistrictId);
                    Taxi taxi = TaxiDetailsVM.FromTaxiDetailsVM(newTaxi, district);
                    taxi.Available = true;
                    this.Data.Taxies.Add(taxi);
                    this.Data.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View("Create", newTaxi);
            }
            catch
            {
                return View();
            }
        }

        // GET: Management/Taxies/Edit/5
        [HttpGet]
        public ActionResult Edit(int taxiId)
        {
            try
            {
                var taxiVM = this.Data.Taxies
               .SearchFor(t => t.TaxiId == taxiId)
               .Project().To<TaxiDetailsVM>()
                    //.Select(TaxiDetailsVM.FromTaxiDataModel)
               .FirstOrDefault();
                var districts = this.populator.GetDistricts();
                ViewBag.Districts = districts;
                return PartialView("Edit", taxiVM);
            }
            catch
            {
                return Content("Error occured!");
            }
        }

        // POST: Management/Taxies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TaxiDetailsVM taxiDetailsVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    District district = this.Data.Districts.All().First(d => d.DistrictId == taxiDetailsVM.AssignDistrictId);
                    if (district == null)
                    {
                        TempData["Error"] = "District not found!";
                        return PartialView("Edit", taxiDetailsVM);
                    }

                    var taxi = this.Data.Taxies.Find(taxiDetailsVM.TaxiId);
                    taxi.Plate = taxiDetailsVM.Plate;
                    taxi.Seats = taxiDetailsVM.Seats;
                    taxi.Luggage = taxiDetailsVM.Luggage;
                    taxi.District = district;
                    taxi.Available = taxiDetailsVM.Available;
                    this.Data.Taxies.Update(taxi);
                    this.Data.SaveChanges();

                    return JavaScript("alert('Success')");
                }

                var districts = this.populator.GetDistricts();
                ViewBag.Districts = districts;

                return PartialView("Edit", taxiDetailsVM);
            }
            catch
            {
                return Content("Error occured!");
            }
        }

        // GET: Management/Taxies/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var taxi = this.Data.Taxies.Find(id);
            if (taxi == null)
            {
                TempData["Error"] = "Taxi not found!";
                return RedirectToAction("Index");
            }

            if (taxi.Available == false)
            {
                TempData["Error"] = "Taxi is not available for decommissioning!!";
                return RedirectToAction("Index");
            }

            var plate = taxi.Plate;
            this.Data.Taxies.Delete(taxi);
            this.Data.SaveChanges();

            TempData["Success"] = "Taxi " + plate + " has been decommisioned";
            return RedirectToAction("Index");
        }

    }
}
