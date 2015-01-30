using Get_A_Taxi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Get_A_Taxi.Web.Areas.Administration.ViewModels
{
    public class RolesEditVM
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        [Required]
        [Display(Name="Име")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        
      // public ICollection<SelectListItem> RoleIds { get; set; }

        [Required]
      //  public ICollection<string> UserRoles { get; set; }
        public ICollection<SelectListItem> UserRoles { get; set; }

        public RolesEditVM()
        {
         //   this.RoleIds = new HashSet<SelectListItem>();
        //    this.UserRoles = new HashSet<string>();
            this.UserRoles = new HashSet<SelectListItem>();
        }

        public static Expression<Func<ApplicationUser, RolesEditVM>> FromApplicationUserModel
        {
            get
            {
                return x => new RolesEditVM
                {
                    Id = x.Id,
                    Name = x.UserName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    
                    // UserRoles = Roles.GetRolesForUser(x.UserName).FirstOrDefault()
                    //UserRoles = x.Roles.ToString()
                };
            }
        }
    }
}