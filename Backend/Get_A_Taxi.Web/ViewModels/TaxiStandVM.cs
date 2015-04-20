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
    public class TaxiStandVM : IMapFrom<TaxiStand>
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int TaxiStandId { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(50)]
        public string Alias { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int DistrictId { get; set; }

        [Display(Name = "District")]
        public string DistrictTitle { get; set; }

        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<TaxiStand, TaxiStandVM>()
                .ForMember(vm => vm.DistrictTitle, opt => opt.MapFrom(m => m.District.Title));
        }
    }
}