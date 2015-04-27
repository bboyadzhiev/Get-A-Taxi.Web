using AutoMapper;
using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Get_A_Taxi.Web.ViewModels
{
    public class UserDetailsVM : UserVM, IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        [Required]
        [StringLength(20)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "User Address")]
        public string DefaultAddress { get; set; }

        // !!! ////[Required]
        [HiddenInput(DisplayValue = false)]
        public int DistritId { get; set; }
        [Display(Name = "Set Employees's District")]
        public IEnumerable<SelectListItem> DistrictsList { get; set; }

        [Display(Name = "User Photo")]
        public Photo Photo { get; set; }

        [Required]
        [Display(Name = "employee role")]
        public string[] SelectedRoleIds { get; set; }

        [Display(Name = "Set Employees's roles")]
        public IEnumerable<SelectListItem> UserRoles { get; set; }

        public UserDetailsVM()
        {
            this.UserRoles = new HashSet<SelectListItem>();
        }
        //public new void CreateMappings(IConfiguration configuration)
        //{
        //    base.CreateMappings(configuration);
        //    configuration.CreateMap<ApplicationUser, UserDetailsVM>()
        //        .ForMember(vm => vm.DistritId, opt => opt.MapFrom(m => m.District == null ? 0 : m.District.DistrictId));
                
        //}
        public override void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ApplicationUser, UserDetailsVM>()
                .ForMember(m => m.FullName, opt => opt.MapFrom(t => t.FirstName + " " + t.MiddleName + " " + t.LastName))
                .ForMember(m => m.District, opt => opt.MapFrom(t => t.District != null ? t.District.Title : null))
                .ForMember(vm => vm.DistritId, opt => opt.MapFrom(m => m.District != null ? m.District.DistrictId : -1));
        }
    }
}