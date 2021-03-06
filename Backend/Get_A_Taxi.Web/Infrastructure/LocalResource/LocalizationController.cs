﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Get_A_Taxi.Web.Infrastructure.Security;

namespace Get_A_Taxi.Web.Infrastructure.LocalResource
{
    [RequireHttpsMVC]
    public class LocalizationController : Controller
    {
        protected void SetCurrentCulture()
        {
            int culture = 0;
            if (this.Session == null || this.Session["CurrentCulture"] == null)
            {

                int.TryParse(System.Configuration.ConfigurationManager.AppSettings["Culture"], out culture);
                this.Session["CurrentCulture"] = culture;
            }
            else
            {
                culture = (int)this.Session["CurrentCulture"];
            }
            // calling CultureHelper class properties for setting  
            CultureHelper.CurrentCulture = culture;
        }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            SetCurrentCulture();
            return base.BeginExecuteCore(callback, state);
        }
    }
}