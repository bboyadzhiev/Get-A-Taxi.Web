using Get_A_Taxi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Get_A_Taxi.Web.Infrastructure.Services.Hubs.HubServices
{
    public interface ITaxiesHubService
    {
        IQueryable<Taxi> AllTaxies();
        IQueryable<Taxi> ByDistrict(IQueryable<Taxi> taxies, int districtId);
        IQueryable<Taxi> ByTaxiStand(IQueryable<Taxi> taxies, int taxiStandId);
        IQueryable<Taxi> ByStatus(IQueryable<Taxi> taxies, TaxiStatus status);
        IQueryable<Taxi> OnDuty(IQueryable<Taxi> taxies);
    }
}
