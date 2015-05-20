using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

using Microsoft.AspNet.Identity;
using Get_A_Taxi.Models;

namespace Get_A_Taxi.Web.Infrastructure
{
    public class AuthorizeWebApiAttribute : AuthorizeAttribute
    {
        public UserRoles UserRole { get; set; }
        public UserRoles SecondRole { get; set; }

        public override async Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (UserRole != 0)
            {
                Roles = UserRole.ToString();
                if (SecondRole != 0)
                {
                    Roles = Roles + "," + SecondRole.ToString();
                }
            }

            if (actionContext.ActionDescriptor.GetCustomAttributes<System.Web.Http.AllowAnonymousAttribute>().Any()) return;

            base.OnAuthorization(actionContext);

            Guid userId;

            if (actionContext.RequestContext.Principal.Identity.IsAuthenticated
                && Guid.TryParse(actionContext.RequestContext.Principal.Identity.GetUserId(), out userId))
            {
               // if (actionContext.Request.Properties["userId"] == null) { 
                    actionContext.Request.Properties.Add("userId", actionContext.RequestContext.Principal.Identity.GetUserId());
               // }
            }
        }

        //public override void OnAuthorization(HttpActionContext actionContext)
        //{
        //    if (actionContext.ActionDescriptor.GetCustomAttributes<System.Web.Http.AllowAnonymousAttribute>().Any()) return;

        //    base.OnAuthorization(actionContext);

        //    Guid userId;

        //    if (actionContext.RequestContext.Principal.Identity.IsAuthenticated
        //        && Guid.TryParse(actionContext.RequestContext.Principal.Identity.GetUserId(), out userId))
        //    {
        //        //actionContext.Request.Properties.Add("userId", actionContext.RequestContext.Principal.Identity.GetUserId());
        //    }
        //}
    }
}