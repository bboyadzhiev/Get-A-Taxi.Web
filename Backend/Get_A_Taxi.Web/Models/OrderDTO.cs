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
    public class OrderDTO : IMapFrom<Order>
    {
        [JsonProperty(PropertyName = "orderId")]
        public int OrderId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "startLat")]
        public double OrderLattitude { get; set; }

        [Required]
        [JsonProperty(PropertyName = "startLng")]
        public double OrderLongitude { get; set; }

        [StringLength(100)]
        [Display(Name = "Order address")]
        [JsonProperty(PropertyName = "start")]
        public string OrderAddress { get; set; }

         [JsonProperty(PropertyName = "endLat")]
        public double DestinationLattitude { get; set; }

         [JsonProperty(PropertyName = "endLng")]
        public double DestinationLongitude { get; set; }

        [StringLength(100)]
        [Display(Name = "Destination address")]
        [JsonProperty(PropertyName = "end")]
        public string DestinationAddress { get; set; }
        
        [Display(Name = "Customer comment")]
        [JsonProperty(PropertyName = "custComment")]
        public string UserComment { get; set; }
    }
}