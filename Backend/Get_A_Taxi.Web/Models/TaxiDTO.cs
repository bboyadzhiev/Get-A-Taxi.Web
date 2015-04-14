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
    public class TaxiDTO : IMapFrom<Taxi>
    {
        [JsonProperty(PropertyName = "taxiId")]
        public int TaxiId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "lat")]
        public double Latitude { get; set; }

        [Required]
        [JsonProperty(PropertyName = "lon")]
        public double Longitude { get; set; }

        [Required]
        [JsonProperty(PropertyName = "status")]
        public TaxiStatus Status { get; set; }

    }
}