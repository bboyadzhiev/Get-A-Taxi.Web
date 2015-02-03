using Get_A_Taxi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Get_A_Taxi.Web.ViewModels
{
    public class TaxiItemVM
    {
        [Key]
        public int TaxiId { get; set; }

        [Required]
        [Display(Name="Plate")]
        public string Plate { get; set; }

        [Required]
        public int DistrictId { get; set; }

        [Display(Name = "Taxi's District")]
        public string DistrictTitle { get; set; }

        public string DriverId { get; set; }

        [Display(Name = "Driver's Phone")]
        public string DriverPhoneNumber { get; set; }

        [Display(Name = "Driver Name")]
        public string DriverName { get; set; }

        [DefaultValue(true)]
        [Display(Name = "Is available")]
        public bool Available { get; set; }

        public TaxiItemVM()
        {
            Available = true;
        }

        public static Expression<Func<Taxi, TaxiItemVM>> FromTaxiDataModel
        {
            get
            {
                return x => new TaxiItemVM
                {
                    TaxiId = x.TaxiId,
                    Plate = x.Plate,
                    DistrictTitle = x.District.Title,
                    DriverId = x.Driver.Id,
                    DriverName = x.Driver.FirstName + " " + x.Driver.LastName,
                    DriverPhoneNumber = x.Driver.PhoneNumber,
                    Available = x.Available
                };
            }
        }
    }
}