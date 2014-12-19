using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Get_A_Taxi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public enum UserType
        {
            Administrator = 0,
            Operator = 1,
            Driver = 2,
            Client = 3
        }

        public ApplicationUser()
        {
            this.Orders = new HashSet<Order>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        public string DefaultAddress { get; set; }
        //public virtual Location CurrentLocation { get; set; }
        public double Lattitude { get; set; }
        public double Longitude { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
