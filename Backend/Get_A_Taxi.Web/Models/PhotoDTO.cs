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
    public class PhotoDTO : IMapFrom<Photo>
    {
        [JsonProperty(PropertyName = "photoId")]
        public int Id { get; set; }

        [Required]
        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }

        [Required]
        [StringLength(3)]
        [JsonProperty(PropertyName = "fileExtension")]
        public string FileExtension { get; set; }
    }
}