using Get_A_Taxi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Get_A_Taxi.Web.ViewModels
{
    public class UserInfoVM
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; }

        [Display(Name = "User Photo")]
        public Photo Photo { get; set; }

        [Display(Name = "District")]
        public string District { get; set; }

        public static Expression<Func<ApplicationUser, UserInfoVM>> FromApplicationUserModel
        {
            get
            {
                return x => new UserInfoVM
                {
                    Id = x.Id,
                    Name = x.FirstName + " " + x.LastName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    Photo  = x.Photo,
                    District = x.District.Title
                };
            }
        }
    }
}