using Get_A_Taxi.Data;
using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.Services.Base;
using Get_A_Taxi.Web.Infrastructure.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Get_A_Taxi.Web.Infrastructure.Services
{
    public class TaxiStandService : BaseService, ITaxiStandService
    {
        public TaxiStandService(IGetATaxiData data)
            :base(data)
        {
        }

        public IQueryable<TaxiStand> GetAll()
        {
            return this.Data.Stands.All();
        }

        public IQueryable<TaxiStand> ByAlias(IQueryable<TaxiStand> taxiStands, string alias)
        {
            return taxiStands.Where(ts => ts.Alias.ToLower().Contains(alias));
        }

        public IQueryable<TaxiStand> ByDistrict(IQueryable<TaxiStand> taxiStands, int districtId)
        {
            return taxiStands.Where(ts => ts.District.DistrictId == districtId);
        }

        public IQueryable<TaxiStand> ByNearestLocation(IQueryable<TaxiStand> taxiStands, double lat, double lng)
        {
            return taxiStands.OrderBy(ts => ((ts.Latitude - lat) + (ts.Longitude - lng)));
        }
    }
}