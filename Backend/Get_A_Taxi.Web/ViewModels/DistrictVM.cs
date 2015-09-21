using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.LocalResource;
using Get_A_Taxi.Web.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
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
        [Display(Name = "Title", ResourceType = typeof(Resource))]
        public string Title { get; set; }

       // [DisplayFormat(DataFormatString = "{0:0.000000000}", ApplyFormatInEditMode = true)]
        [Required]
        [Display(Name = "Latitude", ResourceType = typeof(Resource))]
        public double CenterLatitude { get; set; }

      //  [DisplayFormat(DataFormatString = "{0:0.000000000}", ApplyFormatInEditMode = true)]
        [Required]
        [Display(Name = "Longitude", ResourceType = typeof(Resource))]
        public double CenterLongitude { get; set; }

        [DefaultValue(10)]
        [Display(Name = "Zoom", ResourceType = typeof(Resource))]
        public float MapZoom { get; set; }
    }
}