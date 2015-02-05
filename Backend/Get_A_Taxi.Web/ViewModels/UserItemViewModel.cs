using AutoMapper;
using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.Mapping;
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
    public class UserItemViewModel : UserVM, IHaveCustomMappings
    {

        [Required]
        [Display(Name = "Roles")]
        public List<string> RoleIds { get; set; }

        //public static Expression<Func<ApplicationUser, UserItemViewModel>> FromApplicationUserModel
        //{ 
        //    get
        //    {
        //        return x => new UserItemViewModel
        //        {
        //            Id = x.Id,
        //            FullName = x.FirstName +" " + x.LastName,
        //            Email = x.Email,
        //            PhoneNumber = x.PhoneNumber,
        //            District = x.District.Title,
        //           // RoleId = x.Roles.FirstOrDefault().RoleId,
        //            RoleIds = x.Roles.Where(r => r.UserId == x.Id).Select(r=>r.RoleId).ToList()
        //            // UserRoles = Roles.GetRolesForUser(x.UserName).FirstOrDefault()
        //            //UserRoles = x.Roles.ToString()
        //        };
        //    }
        //}
        public new void CreateMappings(IConfiguration configuration)
        {
            base.CreateMappings(configuration);
            configuration.CreateMap<ApplicationUser, UserItemViewModel>()
                .ForMember(m=>m.RoleIds, opt=>opt.MapFrom (t=> t.Roles.Where(r=>r.UserId == t.Id).Select(r=>r.RoleId).ToList()));
        }
        
    }
}