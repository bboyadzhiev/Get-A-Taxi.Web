using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Get_A_Taxi.Web.ViewModels
{
    public class PhotoVM: IMapFrom<Photo>
    {
        [Required]
        public byte[] Content { get; set; }

        [Required]
        [StringLength(4)]
        public string FileExtension { get; set; }

        [Required]
        public int Width { get; set; }

        [Required]
        public int Height { get; set; }
    }
}