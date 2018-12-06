using Get_A_Taxi.Data;
using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.Authorization;
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
using System.Net;
using Get_A_Taxi.Web.Infrastructure.Services;
using System.IO;
using System.Drawing;

namespace Get_A_Taxi.Web.Areas.Management.Controllers
{
    /// <summary>
    /// Taxies management controller
    /// Only users with a role of Administrator or Manager are allowed
    /// </summary>
    [AuthorizeRoles(UserRole = UserRoles.Administrator, SecondRole = UserRoles.Manager)]
    public class TaxiesController : BaseController
    {
        private ITaxiService TaxiService;
        private IAccountService AccountService;
        private const int TAXI_RESULTS_DEFAULT_COUNT = 10;

        public TaxiesController(IGetATaxiData data, IDropDownListPopulator populator)
            : base(data, populator)
        {
            this.TaxiService = new TaxiService(data);
            this.AccountService = new AccountService(data);
        }


        // GET: Management/Taxies
        /// <summary>
        /// Get top 10 taxies in user's District, ordered by plate
        /// </summary>
        /// <returns>A view with a taxies list view model</returns>
        public ActionResult Index()
        {
            var district = UserProfile.District;
            var taxiesListVM = this.TaxiService.AllTaxies()
                .Where(t => t.District.DistrictId == UserProfile.District.DistrictId)
                .OrderBy(t => t.Plate)
                .Take(TAXI_RESULTS_DEFAULT_COUNT)
                .Project().To<TaxiItemVM>()
                .ToList();
            return View("Taxies", taxiesListVM);
        }

        // GET: Management/Taxies/Details/5
        /// <summary>
        /// Get the details of a taxi
        /// </summary>
        /// <param name="taxiId">The taxi's id</param>
        /// <returns>A partial view with the found taxi view model</returns>
        [HttpGet]
        public ActionResult Details(int taxiId)
        {
            var driverRole = this.RoleManager.FindByNameAsync(UserRoles.Driver.ToString()).Result;
            var driversOnly = new List<SelectListItem>(){
               new SelectListItem(){
                   Text = "Drivers only",
                   Value = driverRole.Id,
                   Selected = true
               }
            };
            ViewBag.DistrictsList = this.populator.GetDistricts();
            ViewBag.UserRoles = driversOnly;
            var taxiVM = this.TaxiService.AllTaxies().Where(t => t.TaxiId == taxiId).AsQueryable().Project().To<TaxiDetailsVM>().FirstOrDefault();
            return PartialView("_TaxiDetailsPartialView", taxiVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchTaxi(string plate, string driver, string district)
        {
            IQueryable<Taxi> taxies = this.Data.Taxies.All();
            if (!String.IsNullOrEmpty(driver))
            {
                var driverRoleId = this.RoleManager.Roles.Where(r => r.Name == UserRoles.Driver.ToString()).FirstOrDefault().Id;
                var drivers = this.Data.Users.All().Where(u => (u.Roles.Select(y => y.RoleId).Contains(driverRoleId)));
                drivers = drivers.Where(u => u.FirstName.ToLower().Contains(driver) || u.LastName.ToLower().Contains(driver));
                taxies = taxies.Where(t => drivers.Contains(t.Driver));
            }

            //IQueryable<Taxi> taxies = this.TaxiService.AllTaxies();
            //if (!String.IsNullOrEmpty(driver))
            //{
            //    var driverRoleId = this.RoleManager.Roles.Where(r => r.Name == UserRoles.Driver.ToString()).FirstOrDefault().Id;
            //    var drivers = this.AccountService.AllUsers();
            //    drivers = this.AccountService.WithRole(drivers, driverRoleId);//
            //    drivers = drivers.Where(u => u.FirstName.ToLower().Contains(driver) || u.LastName.ToLower().Contains(driver));
            //    taxies = taxies.Where(t => drivers.Contains(t.Driver));
            //}

            if (!String.IsNullOrEmpty(plate))
            {
                taxies = this.TaxiService.WithPlateLike(taxies, plate);
            }

            if (!String.IsNullOrEmpty(district))
            {
                taxies = this.TaxiService.WithDistrictTitleLike(taxies, district);
            }

            var taxiesListVM = taxies
                .Take(TAXI_RESULTS_DEFAULT_COUNT)
                .Project().To<TaxiItemVM>()
                .ToList();
            return PartialView("_TaxiesListPartialView", taxiesListVM);
        }

        //[HttpPost]
        //public ActionResult SearchDriver(string driverName, string district)
        //{
        //    var driversListVM = this.taxiService.GetDriversByNameAndDistrict(driverName, district, this.RoleManager)
        //       .Project().To<UserItemViewModel>()
        //        //.Select(UserItemViewModel.FromApplicationUserModel)
        //        .ToList();
        //    //var roleItems = this.GetRolesSelectList();
        //    var roleItems = this.populator.GetRoles(this.RoleManager);
        //    ViewBag.UserRoles = roleItems;
        //    return PartialView("_DriversListPartialView", driversListVM);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search([Bind(Include = "FirstName,MiddleName,LastName,DistritId")] UserSearchVM userSearchVM)
        {
            var result = this.AccountService.AllUsers();

            var driverRole = this.RoleManager.FindByNameAsync(UserRoles.Driver.ToString()).Result;
            result = this.AccountService.WithRole(result, driverRole.Id.ToString());
            if (ModelState.IsValid)
            {
                if (userSearchVM.DistritId != 0)
                {
                    var district = this.Data.Districts.SearchFor(d => d.DistrictId == userSearchVM.DistritId).FirstOrDefault();
                    result = this.AccountService.WithDistrictLike(result, district.Title);
                }
                if (userSearchVM.FirstName != null)
                {
                    result = this.AccountService.WithFirstNameLike(result, userSearchVM.FirstName);
                }
                if (userSearchVM.MiddleName != null)
                {
                    result = this.AccountService.WithMiddletNameLike(result, userSearchVM.MiddleName);
                }
                if (userSearchVM.LastName != null)
                {
                    result = this.AccountService.WithLastNameLike(result, userSearchVM.LastName);
                }
            }

            var accountsVM = result
                .Project().To<UserItemViewModel>()
                 .Take(TAXI_RESULTS_DEFAULT_COUNT)
                .ToList();
            ViewBag.UserRoles = this.populator.GetRoles(this.RoleManager);

            return this.PartialView("_DriversListPartialView", accountsVM);
        }

        // GET: Management/Taxies/UserDetails/{string(guid)}
        [HttpGet]
        public ActionResult DriverDetails(string driverId)
        {
            var result = this.AccountService.AllUsers();
            var driverRole = this.RoleManager.FindByNameAsync(UserRoles.Driver.ToString()).Result;
            result = this.AccountService.WithRole(result, driverRole.Id.ToString());

            if (result.FirstOrDefault(u => u.Id == driverId) == null)
            {
                return HttpNotFound();
            }
            var accountInfoVM = result.Project().To<UserDetailsVM>().FirstOrDefault();

            // Photo sizes
            accountInfoVM.Photo.Width = 240;
            accountInfoVM.Photo.Height = 320;
            return PartialView("_UserInfoPartialView", accountInfoVM);
        }

        private Image ConvertByteArrayToImage(byte[] bytes)
        {
            Image image;

            using (MemoryStream ms = new MemoryStream(bytes, true))
            {
                ms.Position = 0;

                ms.Write(bytes, 0, bytes.Length);
                image = Image.FromStream(ms);
            }

            return image;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignDriver(string userId, int taxiId)
        {
            if (userId == null)
            {
                return RedirectToAction("Index");
            }
            var driverRoleId = this.RoleManager.Roles
                .Where(r => r.Name == UserRoles.Driver.ToString())
                .FirstOrDefault().Id;

            var taxi = this.Data.Taxies.All()
                .Where(t => t.TaxiId == taxiId && t.Driver == null)
                .FirstOrDefault();
            if (taxi == null)
            {
                TempData["Error"] = "Taxi not found!";
                return RedirectToAction("Index");
            }
            var driver = this.Data.Users.All()
                .FirstOrDefault(u => u.Id == userId);
            var taxiWithThisDriver = this.Data.Taxies.SearchFor(t => t.Driver.Id == driver.Id).FirstOrDefault();
            if (taxiWithThisDriver != null)
            {
                TempData["Error"] = "This driver is assigned to another taxi: " + taxiWithThisDriver.Plate + "!";
                return RedirectToAction("Index");
            }

            if (taxi.Status != TaxiStatus.Unassigned)
            {
                TempData["Error"] = "Taxi not available for driver change!";
                return RedirectToAction("Index");
            }

            if (!driver.Roles.Select(d => d.RoleId).Contains(driverRoleId))
            {
                TempData["Error"] = "Driver not found!";
                return RedirectToAction("Index");
            }

            taxi.Driver = driver;
            // HACK Check for conflicts!
            taxi.Status = TaxiStatus.OffDuty;
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
                if(taxi.Status != TaxiStatus.Unassigned){
                    TempData["Error"] = "Taxi not available for driver change!";
                    return RedirectToAction("Index");
                }

                taxi.Driver = null;
                taxi.Status = TaxiStatus.Unassigned;
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
               .FirstOrDefault();
                var districts = this.populator.GetDistricts();
                ViewBag.Districts = districts;
                return PartialView("Edit", taxiVM);
            }
            catch (Exception e)
            {
                return Content("Error occured: " + e.ToString());
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
                    var taxi = this.Data.Taxies.Find(taxiDetailsVM.TaxiId);
                    if(taxi.Status != TaxiStatus.Unassigned || taxi.Status != TaxiStatus.Decommissioned)
                    {
                        TempData["Error"] = "Taxi should be Unassigned or Decommissioned to be changed!";
                        return PartialView("Edit", taxiDetailsVM);
                    }
                    taxi.Plate = taxiDetailsVM.Plate;
                    taxi.Seats = taxiDetailsVM.Seats;
                    taxi.Luggage = taxiDetailsVM.Luggage;
                    if (taxiDetailsVM.AssignDistrictId != 0)
                    {
                        District district = this.Data.Districts.All().First(d => d.DistrictId == taxiDetailsVM.AssignDistrictId);
                        if (district == null)
                        {
                            TempData["Error"] = "District not found!";
                            return PartialView("Edit", taxiDetailsVM);
                        }
                        taxi.District = district;
                    }
                    taxi.Status = TaxiStatus.Unassigned;
                    this.Data.Taxies.Update(taxi);
                    this.Data.SaveChanges();

                    return JavaScript("alert('Success')");
                }

                var districts = this.populator.GetDistricts();
                ViewBag.Districts = districts;

                return PartialView("Edit", taxiDetailsVM);
            }
            catch (Exception e)
            {
                return Content("Error occured:" + e.ToString());
            }
        }

        // GET: Management/Taxies/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {

            var taxi = this.Data.Taxies.SearchFor(t => t.TaxiId == id).FirstOrDefault();
            if (taxi == null)
            {
                TempData["Error"] = "Taxi not found!";
                return RedirectToAction("Index");
            }

            if (taxi.Status != TaxiStatus.Unassigned)
            {
                TempData["Error"] = "Taxi is not available for decommissioning!!";
                return RedirectToAction("Index");
            }

            var taxiVM = this.Data.Taxies.All().Where(t => t.TaxiId == id).Project().To<TaxiDetailsVM>().FirstOrDefault();

            return View(taxiVM);

        }

        // POST: Management/Taxies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            var taxi = this.Data.Taxies.SearchFor(t => t.TaxiId == id).FirstOrDefault();
            if (taxi == null)
            {
                TempData["Error"] = "Taxi not found!";
                return HttpNotFound();
            }

            taxi.Status = TaxiStatus.Decommissioned;
            var plate = taxi.Plate;

            this.Data.Taxies.Update(taxi);
            this.Data.Taxies.SaveChanges();

            TempData["Success"] = "Taxi " + plate + " has been decommisioned!";
            return RedirectToAction("Index");
        }
    }
}
