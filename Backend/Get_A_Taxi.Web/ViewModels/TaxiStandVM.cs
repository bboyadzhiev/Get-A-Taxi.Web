using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.LocalResource;
using Get_A_Taxi.Web.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Get_A_Taxi.Web.ViewModels
{
    public class TaxiStandVM : IMapFrom<TaxiStand>, IHaveCustomMappings
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int TaxiStandId { get; set; }

        [Required]
        [Display(Name = "Latitude", ResourceType = typeof(Resource))]
        public double Latitude { get; set; }

        [Required]
        [Display(Name = "Longitude", ResourceType = typeof(Resource))]
        public double Longitude { get; set; }

        [StringLength(100)]
        [Display(Name = "Address", ResourceType = typeof(Resource))]
        public string Address { get; set; }

        [StringLength(50)]
        [Display(Name = "Alias", ResourceType = typeof(Resource))]
        public string Alias { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int DistrictId { get; set; }

        [Display(Name = "District", ResourceType = typeof(Resource))]
        public string DistrictTitle { get; set; }

         [Display(Name = "Active", ResourceType = typeof(Resource))]
        public bool Active { get; set; }

        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<TaxiStand, TaxiStandVM>()
                .ForMember(vm => vm.DistrictTitle, opt => opt.MapFrom(m => m.District.Title));
        }
    }
}