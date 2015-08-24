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
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Text;

namespace Get_A_Taxi.Web.Infrastructure.Services
{
    public class AccountService : BaseService, IAccountService
    {

        public AccountService(IGetATaxiData data)
            : base(data)
        {
        }

        public IQueryable<ApplicationUser> GetEmployees(IQueryable<ApplicationUser> users)
        {
            var employees = users.Where(u => u.Roles.Count != 0);
            return employees;
        }



        public IQueryable<ApplicationUser> AllUsers()
        {
            return this.Data.Users.All();
        }

        public IQueryable<ApplicationUser> WithFirstNameLike(IQueryable<ApplicationUser> users, string nameString)
        {
            var result = users.Where(u => u.FirstName.ToLower().Contains(nameString));
            return result;
        }

        public IQueryable<ApplicationUser> WithMiddletNameLike(IQueryable<ApplicationUser> users, string nameString)
        {
            var result = users.Where(u => u.MiddleName.ToLower().Contains(nameString));
            return result;
        }

        public IQueryable<ApplicationUser> WithLastNameLike(IQueryable<ApplicationUser> users, string nameString)
        {
            var result = users.Where(u => u.LastName.ToLower().Contains(nameString));
            return result;
        }

        public IQueryable<ApplicationUser> WithDistrictLike(IQueryable<ApplicationUser> users, string districtName)
        {
            var result = users.Where(u => u.District.Title.ToLower().Contains(districtName));
            return result;
        }

        public IQueryable<ApplicationUser> WithRole(IQueryable<ApplicationUser> users, string roleId)
        {
            var result = users.Where(u => u.Roles.Select(y => y.RoleId)
                .Contains(roleId));
            return result;
        }

        public IQueryable<ApplicationUser> WithFullNameContaining(IQueryable<ApplicationUser> users, string nameString)
        {
            var name = nameString.ToLower();
            var result = users.Where(u => u.FirstName.ToLower().Contains(name)
                || u.MiddleName.ToLower().Contains(name)
                || u.LastName.ToLower().Contains(name));
            return result;
        }

        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            // HACK: remove
           // return "abcdefghijklmno";
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}