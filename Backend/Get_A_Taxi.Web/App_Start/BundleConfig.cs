﻿using System.Web;
using System.Web.Optimization;

namespace Get_A_Taxi.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/getataxi").Include(
                "~/Scripts/getataxi.users.js"));

            bundles.Add(new ScriptBundle("~/bundles/gmaps").Include(
              "~/Scripts/gmaps.js",
              "~/Scripts/gmaps.map.js"));

            //Used in management of districts and taxistanda
            bundles.Add(new ScriptBundle("~/bundles/gmaps.manage.taxistands").Include(
              "~/Scripts/gmaps.manage.taxistands.js"));

            bundles.Add(new ScriptBundle("~/bundles/gmaps.manage.districts").Include(
              "~/Scripts/gmaps.manage.districts.js"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                "~/Scripts/knockout*"));

            bundles.Add(new ScriptBundle("~/bundles/signalr").Include(
                "~/Scripts/jquery.signalR*"));

            bundles.Add(new ScriptBundle("~/bundles/gmaps.taxies").Include(
                "~/Scripts/gmaps.taxies.js"));

            bundles.Add(new ScriptBundle("~/bundles/gmaps.taxistands").Include(
    "~/Scripts/gmaps.taxistands.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = false;
        }
    }
}
