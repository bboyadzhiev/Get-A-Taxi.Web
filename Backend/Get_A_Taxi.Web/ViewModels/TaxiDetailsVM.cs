﻿using Get_A_Taxi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Get_A_Taxi.Web.ViewModels
{
    public class TaxiDetailsVM
    {
        public int TaxiId { get; set; }

        public string Plate { get; set; }

        public int Seats { get; set; }

        public int Luggage { get; set; }

        public bool Available { get; set; }
        //public virtual Location Location { get; set; }
        public double Lattitude { get; set; }
        public double Longitude { get; set; }

        //public string DriverId { get; set; }
        //public string DriverPhoneNumber { get; set; }
        //public string DriverName { get; set; }

        public UserInfoVM Driver { get; set; }

        public static Expression<Func<Taxi, TaxiDetailsVM>> FromTaxiDataModel
        {
            get
            {
                return x => new TaxiDetailsVM
                {
                    TaxiId = x.TaxiId,
                    Plate = x.Plate,
                    Seats = x.Seats,
                    Luggage = x.Luggage,
                    Available = x.Available,
                    Lattitude = x.Lattitude,
                    Longitude = x.Longitude,
                    //DriverId = x.Driver.Id,
                    //DriverName = x.Driver.FirstName +" " + x.Driver.LastName,
                    //DriverPhoneNumber = x.Driver.PhoneNumber,
                    Driver = new UserInfoVM()
                    {
                        Id = x.Driver.Id,
                        District = x.Driver.District.Title,
                        Email = x.Driver.Email,
                        Name = x.Driver.FirstName + " " + x.Driver.LastName,
                        PhoneNumber = x.Driver.PhoneNumber
                    }
                };
            }
        }
    }
}