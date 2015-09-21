using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.LocalResource;
using Get_A_Taxi.Web.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Get_A_Taxi.Web.ViewModels
{
    public class TaxiVM: IMapFrom<Taxi>, IHaveCustomMappings
    {
        [HiddenInput(DisplayValue = false)]
        public int TaxiId { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors), ErrorMessage = null)]
        [Display(Name = "Plate", ResourceType = typeof(Resource))]
        [StringLength(10)]
        public string Plate { get; set; }

        [Display(Name = "District", ResourceType = typeof(Resource))]
        public string DistrictTitle { get; set; }

        //[DefaultValue(true)]
        //[Display(Name = "Is Available")]
        //public bool Available { get; set; }

        //[DefaultValue(true)]
        //[Display(Name = "Out of service")]
        //public bool OutOfService { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Resource))]
        public TaxiStatus Status { get; set; }

        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<Taxi, TaxiVM>()
                .ForMember(vm => vm.DistrictTitle, opt => opt.MapFrom(m => m.District.Title));
              //  .ForMember(vm => vm.OutOfService, opt => opt.MapFrom(m => m.Status == TaxiStatus.OutOfService));
        }
    }
}