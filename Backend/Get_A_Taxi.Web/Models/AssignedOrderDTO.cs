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
    public class AssignedOrderDTO : OrderDTO, IHaveCustomMappings
    {
        // Properties, updated by taxi assignment
        [JsonProperty(PropertyName = "taxiId")]
        public int TaxiId { get; set; }

        [JsonProperty(PropertyName = "taxiPlate")]
        public string TaxiPlate { get; set; }

        [JsonProperty(PropertyName = "driverPhone")]
        public string DriverPhone { get; set; }

        [JsonProperty(PropertyName = "driverName")]
        public string DriverName { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Order, AssignedOrderDTO>()
                .ForMember(vm => vm.TaxiId, opt => opt.MapFrom(m => (m.AssignedTaxi != null) ? m.AssignedTaxi.TaxiId : -1))
                .ForMember(vm => vm.TaxiPlate, opt => opt.MapFrom(m => (m.AssignedTaxi != null) ? m.AssignedTaxi.Plate : ""))
                .ForMember(vm => vm.DriverPhone, opt => opt.MapFrom(m => (m.AssignedTaxi != null && m.AssignedTaxi.Driver != null) ? m.AssignedTaxi.Driver.PhoneNumber : String.Empty))
                .ForMember(vm => vm.DriverName, opt => opt.MapFrom(m => m.Driver.FirstName + " " + m.Driver.LastName));
        }
    }
}