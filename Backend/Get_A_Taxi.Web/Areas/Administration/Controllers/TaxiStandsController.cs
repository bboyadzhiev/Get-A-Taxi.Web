using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;

using Get_A_Taxi.Data;
using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.Populators;
using Get_A_Taxi.Web.Controllers;
using Get_A_Taxi.Web.Areas.Administration.Controllers;
using Get_A_Taxi.Web.ViewModels;
using Get_A_Taxi.Web.Infrastructure;
using Get_A_Taxi.Web.Infrastructure.Services.Contracts;

namespace Get_A_Taxi.Web.Areas.Administration
{
    [AuthorizeRoles(UserRole = UserRoles.Administrator)]
    public class TaxiStandsController : BaseController
    {
        private const int TAXI_STands_RESULTS_DEFAULT_COUNT = 10;
        private ITaxiStandService _service;
        public TaxiStandsController(IGetATaxiData data, IDropDownListPopulator populator, ITaxiStandService service)
            : base(data, populator)
        {
            this._service = service;
        }

        // GET: Administration/TaxiStands
        public ActionResult Index()
        {
            var taxiStands = this.Data.Stands.All()
                .Take(TAXI_STands_RESULTS_DEFAULT_COUNT)
                .Project().To<TaxiStandVM>().ToList();
            ViewBag.DistrictsList = this.populator.GetDistricts();
            return View(taxiStands);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchStand(string alias, int districtId)
        {
            var result = this._service.GetAll();
            if (alias != null)
            {
                result = this._service.ByAlias(result, alias);
            }
            result = this._service.ByDistrict(result, districtId);

            var taxiStandsList = result.Project().To<TaxiStandVM>()
                .Take(TAXI_STands_RESULTS_DEFAULT_COUNT)
                .ToList();

            return PartialView("_TaxiStandsListPartialView", taxiStandsList);
        }

        // GET: Administration/TaxiStands/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var taxiStandVM = Data.Stands.SearchFor(ts => ts.TaxiStandId == id).Project().To<TaxiStandVM>().FirstOrDefault();
            if (taxiStandVM == null)
            {
                return HttpNotFound();
            }
            return View(taxiStandVM);
        }

        // GET: Administration/TaxiStands/Create
        public ActionResult Create()
        {
            ViewBag.DistrictsList = this.populator.GetDistricts();
            ViewBag.Lat = this.UserProfile.District.CenterLatitude;
            ViewBag.Lng = this.UserProfile.District.CenterLongitude;
            return View();
        }

        // POST: Administration/TaxiStands/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TaxiStandId,Latitude,Longitude,Address,Alias,DistrictId")] TaxiStandVM taxiStandVM)
        {
            if (ModelState.IsValid)
            {
                var taxiStand = Mapper.Map<TaxiStand>(taxiStandVM);
                taxiStand.Active = true;
                this.Data.Stands.Add(taxiStand);
                this.Data.Stands.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(taxiStandVM);
        }

        // GET: Administration/TaxiStands/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var taxiStandVM = this.Data.Stands.SearchFor(ts => ts.TaxiStandId == id).Project().To<TaxiStandVM>().FirstOrDefault();
            if (taxiStandVM == null)
            {
                return HttpNotFound();
            }

            return View(taxiStandVM);
        }

        // POST: Administration/TaxiStands/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TaxiStandId,Latitude,Longitude,Address,Alias,DistrictId")] TaxiStandVM taxiStandVM)
        {
            if (ModelState.IsValid)
            {
                var taxiStand = Mapper.Map<TaxiStand>(taxiStandVM);
                taxiStand.Active = true;
                this.Data.Stands.Update(taxiStand);
                this.Data.Stands.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(taxiStandVM);
        }

        // GET: Administration/TaxiStands/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var taxiStandVM = this.Data.Stands.SearchFor(ts => ts.TaxiStandId == id).Project().To<TaxiStandVM>().FirstOrDefault();
            if (taxiStandVM == null)
            {
                return HttpNotFound();
            }

            return View(taxiStandVM);
        }

        // POST: Administration/TaxiStands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var taxiStand = this.Data.Stands.SearchFor(ts => ts.TaxiStandId == id).FirstOrDefault();
            if (taxiStand == null)
            {
                return HttpNotFound();
            }
            taxiStand.Active = false;
            this.Data.Stands.Update(taxiStand);
            this.Data.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
