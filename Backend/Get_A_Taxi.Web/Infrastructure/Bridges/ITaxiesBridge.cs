using Get_A_Taxi.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Get_A_Taxi.Web.Infrastructure.Bridges
{
    public interface ITaxiesBridge
    {
        event EventHandler<TaxiBridgeDetailedEventArgs> TaxiUpdatedEvent;
        event EventHandler<TaxiBridgeEventArgs> TaxiFreedEvent;
        void UpdateTaxi(TaxiDetailsVM taxiVm, int districtId);
        void FreeTaxi(int taxiId, int districtId);
    }
}
