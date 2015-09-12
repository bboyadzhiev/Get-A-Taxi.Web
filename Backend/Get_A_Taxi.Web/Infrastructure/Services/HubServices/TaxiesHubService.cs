using Get_A_Taxi.Data;
using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Get_A_Taxi.Web.Infrastructure.Services.HubServices
{
    public class TaxiesHubService : BaseService, ITaxiesHubService
    {

        public TaxiesHubService(IGetATaxiData data)
            :base(data)
        {
        }

        public IQueryable<Taxi> AllTaxies()
        {
            return this.Data.Taxies.All();
        }

        public IQueryable<Taxi> ByDistrict(IQueryable<Taxi> taxies, int districtId)
        {
            return taxies.Where(t => t.District.DistrictId == districtId);
        }

        public IQueryable<Taxi> ByTaxiStand(IQueryable<Taxi> taxies, int taxiStandId)
        {
            return taxies.Where(t => t.TaxiStand.TaxiStandId == taxiStandId);
        }

        public IQueryable<Taxi> ByStatus(IQueryable<Taxi> taxies, TaxiStatus status)
        {
            return taxies.Where(t => t.Status == status);
        }


        public IQueryable<Taxi> OnDuty(IQueryable<Taxi> taxies)
        {
            return taxies.Where(t => t.Status == TaxiStatus.Available || t.Status == TaxiStatus.Busy);
        }
    }
}