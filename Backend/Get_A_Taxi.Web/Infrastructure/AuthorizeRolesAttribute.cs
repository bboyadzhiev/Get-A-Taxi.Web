using Get_A_Taxi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Get_A_Taxi.Web.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public UserRoles UserRole { get; set; }
        public UserRoles SecondRole { get; set; }
        


        //public ICollection<UserRoles> UserWithRoles { get; set; }

        //public AuthorizeRolesAttribute()
        //{
        //    this.UserWithRoles = new HashSet<UserRoles>();
        //}

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
            //else
            //{
            //    if (UserWithRoles.Count() > 0)
            //    {
            //        string delimeter = ",";
            //        string roles = String.Join(delimeter, UserWithRoles);
            //        // UserWithRoles.Aggregate((i, j) => new string( i.ToString() + delimeter + j.ToString()));
            //        Roles = roles;
            //    }
            //}

            base.OnAuthorization(filterContext);
        }
    }
}