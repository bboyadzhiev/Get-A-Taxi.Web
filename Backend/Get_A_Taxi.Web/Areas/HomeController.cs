using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Get_A_Taxi.Web.Areas
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
          
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Taxi Management System";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Author";

            return View();
        }
    }
}