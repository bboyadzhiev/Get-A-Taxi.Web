using Get_A_Taxi.Data;
using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.Services.Base;
using Get_A_Taxi.Web.Infrastructure.Services.Contracts;
using Get_A_Taxi.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Get_A_Taxi.Web.Infrastructure.Services
{
    public class TaxiService : BaseService, ITaxiService
    {
        private const int TAXI_RESULTS_DEFAULT_COUNT = 10;

        public TaxiService(IGetATaxiData data)
            : base(data)
        {
        }
       
        public IQueryable<Taxi> AllTaxies()
        {
            var taxies = this.Data.Taxies.All();
            return taxies;
        }

        public IQueryable<Taxi> WithPlateLike(IQueryable<Taxi> taxies, string plate)
        {
            var result = taxies.Where(t => t.Plate.ToLower().Contains(plate.ToLower()));
            return result;
        }

        public IQueryable<Taxi> WithTaxiStandTitleLike(IQueryable<Taxi> taxies, string taxiStandTitle)
        {
            var result = taxies.Where(t => t.TaxiStand.Alias.ToLower().Contains(taxiStandTitle.ToLower()));
            return result;
        }

        public IQueryable<Taxi> WithDistrictTitleLike(IQueryable<Taxi> taxies, string districtTitle)
        {
            var result = taxies.Where(t => (t.District.Title.ToLower().Contains(districtTitle.ToLower())));
            return result;
        }
    }
}