using Get_A_Taxi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Get_A_Taxi.Web.Infrastructure.Services.Contracts
{
    public interface ITaxiStandService
    {
        IQueryable<TaxiStand> GetAll();
        IQueryable<TaxiStand> ByAlias(IQueryable<TaxiStand> taxiStands, string alias);
        IQueryable<TaxiStand> ByDistrict(IQueryable<TaxiStand> taxiStands, int districtId);
        IQueryable<TaxiStand> ByNearestLocation(IQueryable<TaxiStand> taxiStands, double lat, double lng);
        IQueryable<TaxiStand> ActiveOnly(IQueryable<TaxiStand> taxiStands, bool active);
    }
}
