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
    public class TaxiItemVM : TaxiVM
    {
        public UserVM Driver { get; set; }
    }
}