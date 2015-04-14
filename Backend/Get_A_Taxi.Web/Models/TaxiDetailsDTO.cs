using AutoMapper;
using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.Mapping;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Get_A_Taxi.Web.Models
{
    public class TaxiDetailsDTO : TaxiDTO, IHaveCustomMappings
    {
        [JsonProperty(PropertyName = "plate")]
        public string Plate { get; set; }

        [JsonProperty(PropertyName = "driverName")]
        public string DriverName { get; set; }

        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }

        [JsonProperty(PropertyName = "districtId")]
        public int DistrictId { get; set; }

        [JsonProperty(PropertyName = "taxiStandId")]
        public int TaxiStandId { get; set; }

        [JsonProperty(PropertyName = "taxiStandAlias")]
        public string TaxiStandAlias { get; set; }

        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }


        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Taxi, TaxiDetailsDTO>()
                .ForMember(dm => dm.DistrictId, opt => opt.MapFrom(m => m.District.DistrictId))
                .ForMember(dm => dm.TaxiStandId, opt => opt.MapFrom(m => m.TaxiStand.TaxiStandId))
                .ForMember(dm => dm.DriverName, opt => opt.MapFrom(m => m.Driver.FirstName + " " + m.Driver.LastName))
                .ForMember(dm => dm.Phone, opt => opt.MapFrom(m => m.Driver.PhoneNumber))
                .ForMember(dm => dm.TaxiStandAlias, opt => opt.MapFrom(m => m.TaxiStand.Alias));
        }
    }
}