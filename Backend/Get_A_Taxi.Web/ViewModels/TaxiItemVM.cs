using Get_A_Taxi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Get_A_Taxi.Web.ViewModels
{
    public class TaxiItemVM
    {
        public int TaxiId { get; set; }

        [Display(Name="Номер")]
        public string Plate { get; set; }
        public string DistrictTitle { get; set; }
        public string DriverId { get; set; }
        public string DriverPhoneNumber { get; set; }
        public string DriverName { get; set; }
        public bool Available { get; set; }

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