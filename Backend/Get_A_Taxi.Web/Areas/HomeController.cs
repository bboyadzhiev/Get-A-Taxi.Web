using Get_A_Taxi.Data;
using Get_A_Taxi.Web.Infrastructure.LocalResource;
using Get_A_Taxi.Web.Infrastructure.Populators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Get_A_Taxi.Web.Areas
{
    public class HomeController : LocalizationController
    {
        public ActionResult Index()
        {
            if (HttpContext.IsDebuggingEnabled)
            {
                ViewBag.Message = "Demo mode!";
            }
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

        public ActionResult ChangeCurrentCulture(int id)
        {
            //  
            // Change the current culture for this user.  
            //  
            CultureHelper.CurrentCulture = id;
            //  
            // Cache the new current culture into the user HTTP session.   
            //  
            Session["CurrentCulture"] = id;
            //  
            // Redirect to the same page from where the request was made!   
            //  
            return Redirect(Request.UrlReferrer.ToString());
        }  
    }
}