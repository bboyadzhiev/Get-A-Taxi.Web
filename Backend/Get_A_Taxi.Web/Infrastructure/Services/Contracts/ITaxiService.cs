using Get_A_Taxi.Models;
using Get_A_Taxi.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Get_A_Taxi.Web.Infrastructure.Services.Contracts
{
    public interface ITaxiService
    {
        IQueryable<Taxi> AllTaxies();
        IQueryable<Taxi> WithPlateLike(IQueryable<Taxi> taxies, string plate);
        IQueryable<Taxi> WithTaxiStandTitleLike(IQueryable<Taxi> taxies, string taxiStandTitle);
        IQueryable<Taxi> WithDistrictTitleLike(IQueryable<Taxi> taxies, string districtTitle);
    }
}
