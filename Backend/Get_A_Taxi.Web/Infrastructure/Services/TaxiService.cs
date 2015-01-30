using Get_A_Taxi.Data;
using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.Services.Base;
using Get_A_Taxi.Web.Infrastructure.Services.Contracts;
using Get_A_Taxi.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Get_A_Taxi.Web.Infrastructure.Services
{
    public class TaxiService : BaseService, ITaxiService
    {
        private const int TAXI_RESULTS_DEFAULT_COUNT = 10;

        public TaxiService(IGetATaxiData data)
            : base(data)
        {
        }
        public ICollection<TaxiViewModel> GetByPlate(string plate)
        {
            var taxies = this.Data.Taxies.All()
                .Where(t => t.Plate.ToLower().Contains(plate.ToLower()))
                .Select(TaxiViewModel.FromTaxiDataModel)
                .ToList();
            return taxies;
        }

        public ICollection<TaxiViewModel> GetByTaxiStand(string taxiStandAlias)
        {
            var taxies = this.Data.Taxies.All()
                .Where(t => t.TaxiStand.Alias.ToLower().Contains(taxiStandAlias.ToLower()))
                .Select(TaxiViewModel.FromTaxiDataModel)
                .ToList();
            return taxies;
        }

        public IQueryable<Taxi> GetByDriverProperties(string name, string district, ApplicationRoleManager roleManager)
        {
            var drivers = this.GetDriversByNameAndDistrict(name, district, roleManager);
            var taxies = this.Data.Taxies.All().Where(t => drivers.Contains(t.Driver));
            //.Select(TaxiViewModel.FromTaxiDataModel).ToList();
            return taxies;

        }

        public IQueryable<Taxi> GetByAllProps(string plate, string driver, string district, ApplicationRoleManager roleManager)
        {
            var drivers = this.GetDriversByNameAndDistrict(driver, district, roleManager);
            var taxies = this.Data.Taxies.All().Where(t => drivers.Contains(t.Driver) || t.Plate.ToLower().Contains(plate.ToLower()));
            return taxies;  
        }

        public IQueryable<ApplicationUser> GetDriversByNameAndDistrict(string nameText, string districtText, ApplicationRoleManager roleManager)
        {
            var driverRoleId = roleManager.Roles.Where(r => r.Name == UserRoles.Driver.ToString()).FirstOrDefault().Id;

            var drvR = this.Data.Users.All()
                .Where(u => u.Roles.Select(y => y.RoleId)
                    .Contains(driverRoleId));
            var drvD = this.Data.Users.All()
                .Where(u => u.District.Title.ToLower().Contains(districtText.ToLower()));
            var drvN = this.Data.Users.All()
                .Where(u => (u.FirstName.ToLower() + " " + u.MiddleName.ToLower() + " " + u.LastName.ToLower()) == nameText.ToLower());
                    
            var drivers = this.Data.Users.All()
                .Where(u => (u.Roles.Select(y => y.RoleId).Contains(driverRoleId))
                    && ( u.District.Title.ToLower().Contains(districtText.ToLower())
                    || ((u.FirstName.ToLower() + " " + u.LastName.ToLower()) == nameText.ToLower()
                    || (u.FirstName.ToLower() + " " + u.MiddleName.ToLower() + " " + u.LastName.ToLower()) == nameText.ToLower()))
                    );
            //  .Select(AccountItemViewModel.FromApplicationUserModel);
            return drivers;
        }

        public ICollection<AccountItemViewModel> GetFreeDrivers(string name, string district, ApplicationRoleManager roleManager)
        {
             var allDrivers = this.GetDriversByNameAndDistrict(name, district, roleManager);
            var busy = this.Data.Taxies.All()
                .Where(t => t.Driver.Id != null)
                .Select(d=> d.Driver);
            var free = allDrivers.Except(busy)
                .Select(AccountItemViewModel.FromApplicationUserModel)
                .ToList();

            return free;
        }
    }
}