using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Get_A_Taxi.Data;
using Get_A_Taxi.Web.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.Authorization;
using Get_A_Taxi.Web.Infrastructure.Populators;

namespace Get_A_Taxi.Web.Areas.Administration.Controllers
{
    [AuthorizeRoles(UserRole=UserRoles.Administrator)]
    public class DistrictsController : BaseController
    {

        public DistrictsController(IGetATaxiData data, IDropDownListPopulator populator)
            :base(data, populator)
        {
        }
        // GET: Administration/Districts
        public ActionResult Index()
        {
            var districtVMList = this.Data.Districts.All().Project().To<DistrictVM>().ToList();
            return View(districtVMList);
        }

        // GET: Administration/Districts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var districtVM = this.Data.Districts.All().Where(d=>d.DistrictId==id).Project().To<DistrictVM>().FirstOrDefault();
            if (districtVM == null)
            {
                return HttpNotFound();
            }
            return View(districtVM);
        }

        // GET: Administration/Districts/Create
        public ActionResult Create()
        {
            ViewBag.Lat = this.UserProfile.District.CenterLatitude;
            ViewBag.Lng = this.UserProfile.District.CenterLongitude;
            ViewBag.MapZoom = this.UserProfile.District.MapZoom;
            return View();
        }

        // POST: Administration/Districts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DistrictId,Title,CenterLatitude,CenterLongitude,MapZoom")] DistrictVM districtVM)
        {
            if (ModelState.IsValid)
            {
                var district = Mapper.Map<District>(districtVM);
                this.Data.Districts.Add(district);
                this.Data.SaveChanges();
                this.populator.clearDistrictCaches();
                return RedirectToAction("Index");
            }

            return View(districtVM);
        }

        // GET: Administration/Districts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var districtVM = this.Data.Districts.All().Where(d => d.DistrictId == id).Project().To<DistrictVM>().FirstOrDefault();
            if (districtVM == null)
            {
                return HttpNotFound();
            }

            ViewBag.Lat = districtVM.CenterLatitude;
            ViewBag.Lon = districtVM.CenterLongitude;
            ViewBag.MapZoom = districtVM.MapZoom;
            return View(districtVM);
        }

        // POST: Administration/Districts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DistrictId,Title,CenterLatitude,CenterLongitude,MapZoom")] DistrictVM districtVM)
        {
            if (ModelState.IsValid)
            {
                var district = Mapper.Map<District>(districtVM);
                this.Data.Districts.Update(district);
                this.Data.SaveChanges();
                this.populator.clearDistrictCaches();
                return RedirectToAction("Index");
            }
            return View(districtVM);
        }

        // GET: Administration/Districts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var district = this.Data.Districts.All().Where(d => d.DistrictId == id);

            if (district == null)
            {
                return HttpNotFound();
            }

            var districtVM = district.Where(d=>d.Taxies.Any()).Project().To<DistrictVM>().FirstOrDefault();
            if (districtVM != null)
            {
                return HttpNotFound("Cannot delete district with assigned taxies");
            }
            return View(districtVM);
        }

        // POST: Administration/Districts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var district = this.Data.Districts.All().Where(d => d.DistrictId == id).FirstOrDefault();
            this.Data.Districts.Delete(district);
            this.Data.SaveChanges();
            this.populator.clearDistrictCaches();
            return RedirectToAction("Index");
        }

       
    }
}
