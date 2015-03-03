using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Get_A_Taxi.Web.ViewModels;

namespace Get_A_Taxi.Web.Infrastructure.Bridges
{
    public class TaxiBridgeDetailedEventArgs : EventArgs
    {
        public TaxiDetailsVM taxiVm { get; set; }
        public int districtId { get; set; }
    }

    public class TaxiBridgeEventArgs : EventArgs
    {
        public int taxiId { get; set; }
        public int districtId { get; set; }
    }
}
