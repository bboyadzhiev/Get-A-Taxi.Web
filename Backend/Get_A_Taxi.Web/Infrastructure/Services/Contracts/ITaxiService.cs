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
        //ICollection<TaxiStand> GetByTitle(string title);
        //ICollection<TaxiStand> GetByLocation(double lat, double lon);

        ICollection<TaxiViewModel> GetByPlate(string plate);

        ICollection<TaxiViewModel> GetByTaxiStand(string taxiStand);

        IQueryable<Taxi> GetByDriverProperties(string name, string district, ApplicationRoleManager roleManager);

        IQueryable<Taxi> GetByAllProps(string plate, string driver, string district, ApplicationRoleManager roleManager);

        IQueryable<ApplicationUser> GetDriversByNameAndDistrict(string nameText, string districtText, ApplicationRoleManager roleManager);
    }
}
