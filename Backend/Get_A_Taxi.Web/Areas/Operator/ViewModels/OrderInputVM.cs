using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Get_A_Taxi.Web.Areas.Operator.ViewModels
{
    public class OrderInputVM
    {
        [Required]
        public double OrderLattitude { get; set; }
        [Required]
        public double OrderLongitude { get; set; }
        [StringLength(50)]
        public string OrderAddress { get; set; }
   
        public double DestinationLattitude { get; set; }
  
        public double DestinationLongitude { get; set; }
    
        [StringLength(50)]
        public string DestinationAddress { get; set; }
        [Required]
        [StringLength(20)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(20)]
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string UserComment { get; set; }
    }
}