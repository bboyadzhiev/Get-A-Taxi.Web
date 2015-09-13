using Get_A_Taxi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace Get_A_Taxi.Web.Infrastructure.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public UserRoles UserRole { get; set; }
        public UserRoles SecondRole { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (UserRole != 0)
            {
                Roles = UserRole.ToString();
                if (SecondRole != 0)
                {
                    Roles = Roles + "," + SecondRole.ToString();
                }
            }


            base.OnAuthorization(filterContext);
        }

       
    }
}