using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.Mapping;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Get_A_Taxi.Web.Models
{
    public class TaxiDTO : IMapFrom<Taxi>, IHaveCustomMappings
    {
        [JsonProperty(PropertyName = "taxiId")]
        public int TaxiId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "lat")]
        public double Latitude { get; set; }

        [Required]
        [JsonProperty(PropertyName = "lon")]
        public double Longitude { get; set; }

        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }
        
        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<Taxi, TaxiDTO>()
               .ForMember(dm => dm.Status, opt => opt.MapFrom(m => (int)m.Status));
        }
    }
}