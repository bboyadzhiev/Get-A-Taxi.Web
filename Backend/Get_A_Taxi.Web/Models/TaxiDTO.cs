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

        [JsonProperty(PropertyName = "onDuty")]
        public bool OnDuty { get; set; }

        [JsonProperty(PropertyName = "isAvailable")]
        public bool IsAvailable { get; set; }


        //  |IsAvailable    |   OnDuty      |   to TaxiStatus
        //  +---------------+---------------+----------------------
        //  |       0       |       0       | TaxiStatus.OffDuty
        //  |       0       |       1       | TaxiStatus.Busy
        //  |       1       |       0       | TaxiStatus.Unassigned
        //  |       1       |       1       | TaxiStatus.Available
        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<Taxi, TaxiDTO>()
               .ForMember(dm => dm.IsAvailable, opt => opt.MapFrom(m => m.Status == TaxiStatus.Available))
               .ForMember(dm => dm.OnDuty, opt => opt.MapFrom(m => m.Status == TaxiStatus.Available || m.Status == TaxiStatus.Busy));
        }
    }
}