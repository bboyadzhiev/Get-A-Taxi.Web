namespace Get_A_Taxi.Web.Infrastructure.Bridges
{
    using System;
    using Microsoft.AspNet.SignalR.Hubs;
    using Get_A_Taxi.Web.Models;

    public class TaxiesBridge : BaseBridge, ITaxiesBridge
    {
        private event EventHandler<TaxiBridgeDetailedEventArgs> TaxiOnDutyEvent;
        private event EventHandler<TaxiBridgeUpdateEventArgs> TaxiUpdatedEvent;
        private event EventHandler<TaxiBridgeEventArgs> TaxiOffDutyEvent;

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

        private EventHandler<TaxiBridgeDetailedEventArgs> taxiOnDutyHandler;
        private EventHandler<TaxiBridgeUpdateEventArgs> taxiUpdatedHandler;
        private EventHandler<TaxiBridgeEventArgs> taxiOffDutyHandler;

        public TaxiesBridge(IHubConnectionContext<dynamic> clients)
            : base(clients)
        {
            taxiOnDutyHandler = (s, e) =>
            {
                var districtGroup = e.districtId.ToString();
                Clients.Group(districtGroup).taxiOnDuty(e.taxiDM);
            };

            taxiUpdatedHandler = (s, e) =>
            {
                var districtGroup = e.districtId.ToString();
                Clients.Group(districtGroup).taxiUpdated(e.taxiDM);
            };

            taxiOffDutyHandler = (s, e) =>
            {
                var districtGroup = e.districtId.ToString();
                Clients.Group(districtGroup).taxiOffDuty(e.taxiId);
            };
        }

    }
}