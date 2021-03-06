﻿using Microsoft.Owin.Security.OAuth;
using System.Web.Http;

namespace Get_A_Taxi.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
           // config.Filters.Add(new AuthorizeWebApiRolesAttribute());
            // Remove the XML formatter
            //config.Formatters.Remove(config.Formatters.XmlFormatter);

          //  config.Formatters.Add(new BrowserJsonFormatter());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
