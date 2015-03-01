using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Get_A_Taxi.Data;
using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.Populators;
using Get_A_Taxi.Web.Controllers;

namespace Get_A_Taxi.Web.Areas.Administration
{
    public class TaxiStandsController : BaseController
    {
        private const int TAXI_RESULTS_DEFAULT_COUNT = 10;
        public TaxiStandsController(IGetATaxiData data, IDropDownListPopulator populator)
            : base(data, populator)
        {
        }

        // GET: Administration/TaxiStands
        public ActionResult Index()
        {
            return View(Data.Stands.All().ToList());
        }

        // GET: Administration/TaxiStands/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaxiStand taxiStand = Data.Stands.Find(id);
            if (taxiStand == null)
            {
                return HttpNotFound();
            }
            return View(taxiStand);
        }

        // GET: Administration/TaxiStands/Create
        public ActionResult Create()
        {
            ViewBag.Lat = this.UserProfile.District.CenterLattitude;
            ViewBag.Lng = this.UserProfile.District.CenterLongitude;
            return View();
        }

        // POST: Administration/TaxiStands/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TaxiStandId,Lattitude,Longitude,Address,Alias")] TaxiStand taxiStand)
        {
            if (ModelState.IsValid)
            {
                Data.Stands.Add(taxiStand);
                Data.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(taxiStand);
        }

        // GET: Administration/TaxiStands/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaxiStand taxiStand = Data.Stands.Find(id);
            if (taxiStand == null)
            {
                return HttpNotFound();
            }
            return View(taxiStand);
        }

        // POST: Administration/TaxiStands/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TaxiStandId,Lattitude,Longitude,Address,Alias")] TaxiStand taxiStand)
        {
            if (ModelState.IsValid)
            {
             //   Data.Entry(taxiStand).State = EntityState.Modified;
                Data.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(taxiStand);
        }

        // GET: Administration/TaxiStands/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaxiStand taxiStand = Data.Stands.Find(id);
            if (taxiStand == null)
            {
                return HttpNotFound();
            }
            return View(taxiStand);
        }

        // POST: Administration/TaxiStands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TaxiStand taxiStand = Data.Stands.Find(id);
           //Data.Stands.Remove(taxiStand);
            Data.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
             //   Data.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
