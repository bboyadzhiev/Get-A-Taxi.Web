namespace Get_A_Taxi.Web.Infrastructure.Bridges
{
    using System;
    using Microsoft.AspNet.SignalR.Hubs;
    using Get_A_Taxi.Web.Models;

    public class TaxiesBridge : BaseBridge, ITaxiesBridge
    {
        public event EventHandler<TaxiBridgeDetailedEventArgs> TaxiOnDutyEvent;
        public event EventHandler<TaxiBridgeUpdateEventArgs> TaxiUpdatedEvent;
        public event EventHandler<TaxiBridgeEventArgs> TaxiOffDutyEvent;

        public void TaxiUpdated(TaxiDTO taxiDM, int districtId)
        {
            if (TaxiUpdatedEvent != null)
            {
                var args = new TaxiBridgeUpdateEventArgs() { districtId = districtId, taxiDM = taxiDM };
                TaxiUpdatedEvent(this, args);
            }
        }

        public void TaxiOnDuty(TaxiDetailsDTO taxiDM, int districtId)
        {
            if (TaxiOnDutyEvent != null)
            {
                var args = new TaxiBridgeDetailedEventArgs() { districtId = districtId, taxiDM = taxiDM };
                TaxiOnDutyEvent(this, args);
            }
        }

        public void TaxiOffDuty(int taxiId, int districtId)
        {
            if (TaxiOffDutyEvent != null)
            {
                var args = new TaxiBridgeEventArgs() { districtId = districtId, taxiId = taxiId };
                TaxiOffDutyEvent(this, args);
            }
        }

        public TaxiesBridge(IHubConnectionContext<dynamic> clients)
            : base(clients)
        {
        }

    }
}