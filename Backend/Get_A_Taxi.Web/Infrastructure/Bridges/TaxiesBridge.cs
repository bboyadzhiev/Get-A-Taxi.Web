using Get_A_Taxi.Web.ViewModels;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Get_A_Taxi.Web.Infrastructure.Bridges
{
    public class TaxiesBridge : BaseBridge, ITaxiesBridge
    {
        public event EventHandler<TaxiBridgeDetailedEventArgs> TaxiUpdatedEvent;
        public event EventHandler<TaxiBridgeEventArgs> TaxiFreedEvent;

        public void UpdateTaxi(TaxiDetailsVM taxiVm, int districtId)
        {
            if (TaxiUpdatedEvent != null)
            {
                var args = new TaxiBridgeDetailedEventArgs() { districtId = districtId, taxiVm = taxiVm };
                TaxiUpdatedEvent(this, args);
            }
        }

        public void FreeTaxi(int taxiId, int districtId)
        {
            if (TaxiFreedEvent != null)
            {
                var args = new TaxiBridgeEventArgs() { districtId = districtId, taxiId = taxiId };
                TaxiFreedEvent(this, args);
            }
        }

        public TaxiesBridge(IHubConnectionContext<dynamic> clients)
            : base(clients)
        {
        }


    }
}