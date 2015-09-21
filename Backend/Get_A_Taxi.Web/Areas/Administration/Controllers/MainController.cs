using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.Authorization;
using Get_A_Taxi.Web.Infrastructure.LocalResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Get_A_Taxi.Web.Areas.Administration
{
    [AuthorizeRoles(UserRole = UserRoles.Administrator)]
    public class MainController : LocalizationController
    {
        // GET: Administration/Main
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Employees()
        {
            return View("ManageEmployees");
        }
    }
}