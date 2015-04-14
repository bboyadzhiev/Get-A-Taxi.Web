namespace Get_A_Taxi.Web.Infrastructure.Bridges
{
    using System;
    using Get_A_Taxi.Web.Models;
    public class TaxiBridgeDetailedEventArgs : EventArgs
    {
        public TaxiDetailsDTO taxiDM { get; set; }
        public int districtId { get; set; }
    }

    public class TaxiBridgeUpdateEventArgs : EventArgs
    {
        public TaxiDTO taxiDM { get; set; }
        public int districtId { get; set; }
    }

    public class TaxiBridgeEventArgs : EventArgs
    {
        public int taxiId { get; set; }
        public int districtId { get; set; }
    }
}
