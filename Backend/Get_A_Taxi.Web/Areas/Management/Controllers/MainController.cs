﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Get_A_Taxi.Web.Areas.Management.Controllers
{
    public class MainController : Controller
    {
        // GET: Management/Main
        public ActionResult Index()
        {
            return View();
        }
    }
}