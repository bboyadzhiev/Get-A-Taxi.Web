using Get_A_Taxi.Data;
using Get_A_Taxi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Get_A_Taxi.Web.Infrastructure.Populators;
using Get_A_Taxi.Web.Infrastructure.LocalResource;

namespace Get_A_Taxi.Web.Areas
{
    public class BaseController : LocalizationController
    {
        protected IGetATaxiData Data { get; private set; }

        protected ApplicationUser UserProfile { get; private set; }
        protected IDropDownListPopulator populator;

        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public BaseController(IGetATaxiData data, IDropDownListPopulator populator)
        {
            this.Data = data;
            this.populator = populator;
        }

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            this.UserProfile = this.Data.Users.All().Where(u => u.UserName == requestContext.HttpContext.User.Identity.Name).FirstOrDefault();

            return base.BeginExecute(requestContext, callback, state);
        }

        protected ActionResult RedirectToAction<TController>(Expression<Action<TController>> action) where TController : Controller
        {
            var actionBody = (MethodCallExpression)action.Body;
            var methodName = actionBody.Method.Name;

            var controllerName = typeof(TController).Name;
            controllerName = controllerName.Substring(0, controllerName.Length - "Controller".Length);

            return RedirectToAction(methodName, controllerName);
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
    }
}