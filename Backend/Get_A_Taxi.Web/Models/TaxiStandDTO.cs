using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.Mapping;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Get_A_Taxi.Web.Models
{
    public class TaxiStandDTO : IMapFrom<TaxiStand>
    {
        [JsonProperty(PropertyName = "taxiStandId")]
        public int TaxiStandId { get; set; }

        [JsonProperty(PropertyName = "lat")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "lng")]
        public double Longitude { get; set; }

        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "alias")]
        public string Alias { get; set; }
    }
}