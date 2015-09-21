using AutoMapper;
using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.LocalResource;
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
    public class UserItemViewModel : UserVM, IMapFrom<ApplicationUser>, IHaveCustomMappings
    {

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors), ErrorMessage = null)]
        [Display(Name = "Roles", ResourceType = typeof(Resource))]
        public List<string> Roles { get; set; }

        public override void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ApplicationUser, UserItemViewModel>()
                .ForMember(m => m.FullName, opt => opt.MapFrom(t => t.FirstName + " " + t.MiddleName + " " + t.LastName))
                .ForMember(m => m.District, opt => opt.MapFrom(t => t.District.Title))
              .ForMember(m => m.Roles, opt => opt.MapFrom(t => t.Roles.Where(r => r.UserId == t.Id).Select(r => r.RoleId).ToList()));
        }
        
    }
}