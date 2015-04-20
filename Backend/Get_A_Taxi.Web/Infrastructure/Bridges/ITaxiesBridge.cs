namespace Get_A_Taxi.Web.Infrastructure.Bridges
{

    using System;
    using Get_A_Taxi.Web.Models;

    public interface ITaxiesBridge
    {
        void TaxiUpdated(TaxiDTO taxiDM, int districtId);
        void TaxiOnDuty(TaxiDetailsDTO taxiDM, int districtId);
        void TaxiOffDuty(int taxiId, int districtId);
    }
}
