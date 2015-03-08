using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Get_A_Taxi.Web.Models
{
    public class LocationDataModel : IMapFrom<Location>
    {

        public int LocationId { get; set; }

        [Required]
        public double Lattitude { get; set; }
        [Required]
        public double Longitude { get; set; }
        [StringLength(100)]
        public string Address { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
    }
}