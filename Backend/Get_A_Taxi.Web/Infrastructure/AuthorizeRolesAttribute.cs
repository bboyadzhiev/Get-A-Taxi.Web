using Get_A_Taxi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Get_A_Taxi.Web.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public UserRoles UserRole { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (UserRole != 0)
            {
                Roles = UserRole.ToString();
            }

            base.OnAuthorization(filterContext);
        }
    }
}