using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Get_A_Taxi.Web.Models
{
    public class OrderDataModel :IMapFrom<Order>
    {
        public int OrderId { get; set; }

        [Required]
        public double OrderLattitude { get; set; }
        [Required]
        public double OrderLongitude { get; set; }
        [StringLength(100)]
        [Display(Name = "Order address")]
        public string OrderAddress { get; set; }

        public double DestinationLattitude { get; set; }

        public double DestinationLongitude { get; set; }

        [StringLength(100)]
        [Display(Name = "Destination address")]
        public string DestinationAddress { get; set; }
        
        [Display(Name = "User comment")]
        public string UserComment { get; set; }
    }
}