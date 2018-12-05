using AutoMapper;
using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.LocalResource;
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
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors), ErrorMessage = null)]
        [StringLength(20, ErrorMessageResourceName = "AtLeastLength", MinimumLength = 2, ErrorMessageResourceType = typeof(Errors), ErrorMessage = null)]
        [Display(Name = "FirstName", ResourceType = typeof(Resource))]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors), ErrorMessage = null)]
        [StringLength(20, ErrorMessageResourceName = "AtLeastLength", MinimumLength = 2, ErrorMessageResourceType = typeof(Errors), ErrorMessage = null)]
        [Display(Name = "MiddleName", ResourceType = typeof(Resource))]
        public string MiddleName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors), ErrorMessage = null)]
        [StringLength(20, ErrorMessageResourceName = "AtLeastLength", MinimumLength = 2, ErrorMessageResourceType = typeof(Errors), ErrorMessage = null)]
        [Display(Name = "LastName", ResourceType = typeof(Resource))]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors), ErrorMessage = null)]
        [StringLength(50, ErrorMessageResourceName = "AtLeastLength", MinimumLength = 6, ErrorMessageResourceType = typeof(Errors), ErrorMessage = null)]
        [Display(Name = "Address", ResourceType = typeof(Resource))]
        public string DefaultAddress { get; set; }

        // !!! ////[Required]
        [HiddenInput(DisplayValue = false)]
        [Display(Name = "SetEmployeeDistrict", ResourceType = typeof(Resource))]
        public int DistritId { get; set; }

        
        public IEnumerable<SelectListItem> DistrictsList { get; set; }

        [Display(Name = "UserPhoto", ResourceType = typeof(Resource))]
        public PhotoVM Photo { get; set; }

        [Required]
        [Display(Name = "employee role")]
        public string[] SelectedRoleIds { get; set; }

        [Display(Name = "Roles", ResourceType = typeof(Resource))]
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