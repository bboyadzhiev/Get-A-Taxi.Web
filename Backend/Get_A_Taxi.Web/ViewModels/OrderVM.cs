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
    public class OrderVM : IMapFrom<Order>
    {
        [HiddenInput(DisplayValue = false)]
        public int OrderId { get; set; }

        public TaxiVM AssignedTaxi { get; set; }

        public UserVM Customer { get; set; }

        [Display(Name = "Status")]
        public OrderStatus OrderStatus { get; set; }

        [Display(Name = "Order Address")]
        public string OrderAddress { get; set; }

        [Display(Name = "Destination")]
        public string DestinationAddress { get; set; }

        [Display(Name = "Ordered at")]
        public DateTime OrderedAt { get; set; }
    }
}