using Get_A_Taxi.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Get_A_Taxi.Web.Infrastructure.Bridges
{
    public class OrderBridgeDetailedEventArgs : EventArgs
    {
        public OrderDetailsVM order { get; set; }
        public int districtId { get; set; }
    }

    public class OrderBridgeEventArgs : EventArgs
    {
        public int orderId { get; set; }
        public int districtId { get; set; }
    }

    public class OrderBridgeAssignmentEventArgs : EventArgs
    {
        public int orderId { get; set; }
        public int taxiId { get; set; }
        public int districtId { get; set; }
    }
}