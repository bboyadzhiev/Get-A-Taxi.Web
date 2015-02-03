using Get_A_Taxi.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Get_A_Taxi.Web.ViewModels
{
    public class UserItemViewModel
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        [Required]
        public bool Marked { get; set; }

        [Required]
        [Display(Name="Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; }

        [Display(Name = "District")]
        public string District { get; set; }

       // [Required]
       // public string RoleId { get; set; }

        [Required]
        [Display(Name = "Roles")]
        public List<string> RoleIds { get; set; }

        public static Expression<Func<ApplicationUser, UserItemViewModel>> FromApplicationUserModel
        { 
            get
            {
                return x => new UserItemViewModel
                {
                    Id = x.Id,
                    Name = x.FirstName +" " + x.LastName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    District = x.District.Title,
                   // RoleId = x.Roles.FirstOrDefault().RoleId,
                    RoleIds = x.Roles.Where(r => r.UserId == x.Id).Select(r=>r.RoleId).ToList()
                    // UserRoles = Roles.GetRolesForUser(x.UserName).FirstOrDefault()
                    //UserRoles = x.Roles.ToString()
                };
            }
        }

        
    }
}