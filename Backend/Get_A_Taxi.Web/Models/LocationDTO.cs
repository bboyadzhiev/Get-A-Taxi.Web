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
    public class LocationDTO : IMapFrom<Location>
    {
        [JsonProperty(PropertyName = "locationId")]
        public int LocationId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "lat")]
        public double Lattitude { get; set; }

        [Required]
        [JsonProperty(PropertyName = "lon")]
        public double Longitude { get; set; }

        [StringLength(100)]
        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }

        [Required]
        [StringLength(100)]
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
    }
}