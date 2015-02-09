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
    public enum UserRoles
    {
        Administrator = 1,
        Manager = 2,
        Operator = 4,
        Driver = 8,
       // Client = 16 - registered user with no role is a client
    }
    public class ApplicationUser : IdentityUser
    {
        
        public ApplicationUser()
        {
            this.Orders = new HashSet<Order>();
            this.Favorites = new HashSet<Location>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        private ICollection<Order> orders;
        private ICollection<Location> favorites;

        [StringLength(20)]
        public string FirstName { get; set; }
        [StringLength(20)]
        public string MiddleName { get; set; }
        [StringLength(20)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string DefaultAddress { get; set; }


       // public int? PhotoId { get; set; }
        public virtual Photo Photo { get; set; }

        //public int DistrictId { get; set; }
        public virtual District District { get; set; }

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

        public virtual ICollection<Location> Favorites
        {
            get
            {
                return this.favorites;
            }
            set
            {
                this.favorites = value;
            }
        }

    }
}
