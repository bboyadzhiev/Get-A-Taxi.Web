using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.Mapping;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
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


        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
           
            configuration.CreateMap<Taxi, TaxiDetailsDTO>()
                .ForMember(dm => dm.DistrictId, opt => opt.MapFrom(m => m.District.DistrictId))
                .ForMember(dm => dm.DriverName, opt => opt.MapFrom(m => m.Driver.FirstName + " " + m.Driver.LastName))
                .ForMember(dm => dm.Phone, opt => opt.MapFrom(m => m.Driver.PhoneNumber))
                .ForMember(dm => dm.TaxiStandId, opt => opt.MapFrom(m => m.TaxiStand != null ? m.TaxiStand.TaxiStandId : -1))
                .ForMember(dm => dm.TaxiStandAlias, opt => opt.MapFrom(m => m.TaxiStand != null ? m.TaxiStand.Alias : "new"))
               .ForMember(dm => dm.IsAvailable, opt => opt.MapFrom(m => m.Status == TaxiStatus.Available || m.Status == TaxiStatus.Unassigned))
               .ForMember(dm => dm.OnDuty, opt => opt.MapFrom(m => m.Status == TaxiStatus.Available || m.Status == TaxiStatus.Busy));
        }
    }
}