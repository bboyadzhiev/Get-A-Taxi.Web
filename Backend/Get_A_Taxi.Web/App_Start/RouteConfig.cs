using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Get_A_Taxi.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("{desklineweb4}/{*pathInfo}", new { desklineweb4 = @"deskline*.*" });

            routes.IgnoreRoute("{proxytest}/{*pathInfo}", new { proxytest = @"proxytest*.*" });

          //  routes.MapRoute(
          //    name: "ReverseProxy",
          //    url: "proxytest/{controller}/{action}/{id}",
          //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
          //);
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
           
        }
    }
}
