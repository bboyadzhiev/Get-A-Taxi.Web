using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Get_A_Taxi.Web.ViewModels
{
    public class DistrictVM : IMapFrom<District>
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int DistrictId { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        public double CenterLattitude { get; set; }
        [Required]
        public double CenterLongitude { get; set; }

        [DefaultValue(10)]
        public float MapZoom { get; set; }
    }
}