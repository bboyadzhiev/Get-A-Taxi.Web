using Get_A_Taxi.Data;
using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Areas.Administration.ViewModels;
using Get_A_Taxi.Web.Infrastructure.Services.Base;
using Get_A_Taxi.Web.Infrastructure.Services.Contracts;
using Get_A_Taxi.Web.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Get_A_Taxi.Web.Infrastructure.Services
{
    public class AccountService : BaseService, IAccountService
    {

        public AccountService(IGetATaxiData data)
            : base(data)
        {
        }

        public ICollection<AccountItemViewModel> GetAccounts()
        {

            var accounts = this.Data.Users.All()
                .Select(AccountItemViewModel.FromApplicationUserModel)
                .ToList();
            return accounts;
        }

        public ICollection<AccountItemViewModel> GetEmployees(int count)
        {
            
            ICollection<AccountItemViewModel> employees;
            if (count != null)
            {
                 employees = this.Data.Users.All().Where(u => u.Roles.Count != 0)
                     .Take(count)
                    .Select(AccountItemViewModel.FromApplicationUserModel)
                    .ToList();
                return employees;
            }
             employees = this.Data.Users.All().Where(u => u.Roles.Count != 0)
                   .Select(AccountItemViewModel.FromApplicationUserModel)
                   .ToList();
            return employees;
        }

        public ICollection<AccountItemViewModel> GetAccountsByTextSearch(string textToSerach)
        {
            var result = this.Data.Users.All()
                .Where(u => u.UserName.ToLower().Contains(textToSerach.ToLower()) || u.PhoneNumber.Contains(textToSerach))
                .Select(AccountItemViewModel.FromApplicationUserModel).ToList();
            return result;
        }

        public IQueryable<ApplicationUser> GetUserByNames(string name)
        {
            var result = this.Data.Users.All()
                .Where(u => (u.FirstName.ToLower() + " " + u.LastName.ToLower()) == name.ToLower() 
                    || (u.FirstName.ToLower() + " " + u.MiddleName.ToLower() + " " + u.LastName.ToLower()) == name.ToLower());
            return result;
        }



        public ICollection<AccountItemViewModel> GetAccountByDistrict(string districtText)
        {
            var usersInDistrict = this.Data.Users.All()
                .Where(u => u.District.Title.ToLower().Contains(districtText.ToLower()))
                .Select(AccountItemViewModel.FromApplicationUserModel).ToList();
            return usersInDistrict;
        }

        public ICollection<AccountItemViewModel> GetAccountsByRole(UserRoles role, ApplicationRoleManager roleManager)
        {
            var identityRoleId = roleManager.Roles.Where(r => r.Name == role.ToString()).FirstOrDefault().Id;
            var usersInRole = this.Data.Users.All().Where(u => u.Roles.Select(y => y.RoleId)
                .Contains(identityRoleId)).
                Select(AccountItemViewModel.FromApplicationUserModel);
            return usersInRole.ToList();
        }

        public ICollection<AccountItemViewModel> GetAccountsByRoleAndDistrict(int districtId, UserRoles role, ApplicationRoleManager roleManager)
        {
            var identityRoleId = roleManager.Roles.Where(r => r.Name == role.ToString()).FirstOrDefault().Id;
            var usersInRole = this.Data.Users.All().Where(u => u.Roles.Select(y => y.RoleId)
                .Contains(identityRoleId) && u.District.DistrictId == districtId)
                .Select(AccountItemViewModel.FromApplicationUserModel).ToList();
            return usersInRole;
        }

        public ICollection<AccountItemViewModel> GetAccountsByRoleAndDistrict(string districtText, string roleTextSearch, ApplicationRoleManager roleManager)
        {
            var usersInRole = SearchByRoleAndDistrict(districtText, roleTextSearch, roleManager);
            return usersInRole.ToList();
        }

        private IQueryable<AccountItemViewModel> SearchByRoleAndDistrict(string districtText, string roleText, ApplicationRoleManager roleManager)
        {
            var identityRoleId = roleManager.Roles.Where(r => r.Name.ToLower().Contains(roleText.ToLower())).FirstOrDefault().Id;
            var usersInRole = this.Data.Users.All()
                .Where(u => u.Roles.Select(y => y.RoleId)
                    .Contains(identityRoleId) && u.District.Title.ToLower()
                    .Contains(districtText.ToLower()))
                .Select(AccountItemViewModel.FromApplicationUserModel);
            return usersInRole;
        }

        private IQueryable<ApplicationUser> SearchByNameDistrictRole(string nameText, string districtText, string roleText, ApplicationRoleManager roleManager)
        {
            var identityRoleId = roleManager.Roles.Where(r => r.Name.ToLower().Contains(roleText.ToLower())).FirstOrDefault().Id;
            var usersInRole = this.Data.Users.All()
                .Where(u => u.Roles.Select(y => y.RoleId)
                    .Contains(identityRoleId)
                    && u.District.Title.ToLower().Contains(districtText.ToLower())
                    && ((u.FirstName.ToLower() + " " + u.LastName.ToLower()) == nameText.ToLower()
                    || (u.FirstName.ToLower() + " " + u.MiddleName.ToLower() + " " + u.LastName.ToLower()) == nameText.ToLower())
                    );
              //  .Select(AccountItemViewModel.FromApplicationUserModel);
            return usersInRole;
        }

        

    }
}