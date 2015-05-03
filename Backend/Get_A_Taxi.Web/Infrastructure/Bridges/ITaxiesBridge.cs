namespace Get_A_Taxi.Web.Infrastructure.Bridges
{

    using System;
    using Get_A_Taxi.Web.Models;

    /// <summary>
    /// Taxi location and/or status changed bridge
    /// </summary>
    public interface ITaxiesBridge
    {
        /// <summary>
        /// Updates taxi position and/or status
        /// </summary>
        /// <param name="taxiDM">The data model of the taxi location and status</param>
        /// <param name="districtId">The district the taxi operating assigned in</param>
        void TaxiUpdated(TaxiDTO taxiDM, int districtId);

        /// <summary>
        /// Updates 
        /// </summary>
        /// <param name="taxiDM"></param>
        /// <param name="districtId"></param>
        void TaxiOnDuty(TaxiDetailsDTO taxiDM, int districtId);
        void TaxiOffDuty(int taxiId, int districtId);
    }
}
