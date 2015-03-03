using Get_A_Taxi.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Get_A_Taxi.Web.Infrastructure.Bridges
{
    public class OrderBidgeEventArgs : EventArgs
    {
        public OrderDetailsVM order { get; set; }
        public int districtId { get; set; }
    }
}