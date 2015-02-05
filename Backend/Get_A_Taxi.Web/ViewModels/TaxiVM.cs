using Get_A_Taxi.Models;
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

        [Required]
        [Display(Name = "Plate")]
        [StringLength(8)]
        public string Plate { get; set; }

        [Display(Name = "Taxi's District")]
        public string DistrictTitle { get; set; }

        [DefaultValue(true)]
        [Display(Name = "Is Available")]
        public bool Available { get; set; }

        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<Taxi, TaxiVM>()
                .ForMember(vm => vm.DistrictTitle, opt => opt.MapFrom(m => m.District.Title));
        }
    }
}