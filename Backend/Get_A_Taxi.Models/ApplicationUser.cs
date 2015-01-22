using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace Get_A_Taxi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public enum UserType
        {
            Administrator = 0,
            Manager = 1,
            Operator = 2,
            Driver = 3,
            Client = 4
        }

        public ApplicationUser()
        {
            this.orders = new HashSet<Order>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        private ICollection<Order> orders;

        [StringLength(50)]
        public string DefaultAddress { get; set; }
        //public virtual Location CurrentLocation { get; set; }

        public int? PhotoId { get; set; }
        public virtual Photo Photo { get; set; }

        public double Lattitude { get; set; }
        public double Longitude { get; set; }

        public virtual ICollection<Order> Orders
        {
            get
            {
                return this.orders;
            }
            set
            {
                this.orders = value;
            }
        }
    }
}
