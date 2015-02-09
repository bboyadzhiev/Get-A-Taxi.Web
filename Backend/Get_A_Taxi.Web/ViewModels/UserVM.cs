using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Get_A_Taxi.Web.ViewModels
{
    public class UserVM: IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; }

        [Display(Name = "District")]
        public string District { get; set; }

        public virtual void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<ApplicationUser, UserVM>()
                .ForMember(m => m.FullName, opt => opt.MapFrom(t => t.FirstName + " " + t.MiddleName + " " + t.LastName))
                .ForMember(m => m.District, opt => opt.MapFrom(t => t.District.Title));

            //configuration.CreateMap<UserVM, ApplicationUser>()
            //    .ForMember(m => m.FirstName, opt => opt.MapFrom(t => t.FullName.Split(' ').FirstOrDefault()))
            //    .ForMember(m => m.MiddleName, opt => opt.MapFrom(r => r.FullName.Split(' ').ElementAtOrDefault(1)))
            //    .ForMember(m => m.LastName, opt => opt.MapFrom(r => r.FullName.Split(' ').LastOrDefault()));
        }
    }
}