using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Get_A_Taxi.Web.Infrastructure.LocalResource
{
    public class LocalizationController : Controller
    {
        // GET: Localization
        //protected override void ExecuteCore()
        //{
        //    int culture = 0;
        //    if (this.Session == null || this.Session["CurrentCulture"] == null)
        //    {

        //        int.TryParse(System.Configuration.ConfigurationManager.AppSettings["Culture"], out culture);
        //        this.Session["CurrentCulture"] = culture;
        //    }
        //    else
        //    {
        //        culture = (int)this.Session["CurrentCulture"];
        //    }
        //    // calling CultureHelper class properties for setting  
        //    CultureHelper.CurrentCulture = culture;

        //    base.ExecuteCore();
        //}

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