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
    public class OrderDTO : IMapFrom<Order>, IHaveCustomMappings
    {
        [JsonProperty(PropertyName = "orderId")]
        public int OrderId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "startLat")]
        public double OrderLatitude { get; set; }

        [Required]
        [JsonProperty(PropertyName = "startLng")]
        public double OrderLongitude { get; set; }

        [StringLength(100)]
        [JsonProperty(PropertyName = "start")]
        public string OrderAddress { get; set; }

         [JsonProperty(PropertyName = "endLat")]
        public double DestinationLatitude { get; set; }

         [JsonProperty(PropertyName = "endLng")]
        public double DestinationLongitude { get; set; }

        [StringLength(100)]
        [JsonProperty(PropertyName = "end")]
        public string DestinationAddress { get; set; }
        
        [JsonProperty(PropertyName = "custComment")]
        public string UserComment { get; set; }

        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }

        [JsonProperty(PropertyName = "pickupTime")] // in minutes
        public int PickupTime { get; set; }

        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<Order, OrderDTO>()
                .ForMember(vm => vm.Status, opt => opt.MapFrom(m => (int)m.OrderStatus));
        }
    }
}