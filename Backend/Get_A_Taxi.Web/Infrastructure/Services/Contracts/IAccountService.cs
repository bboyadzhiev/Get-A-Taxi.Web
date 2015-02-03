using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Areas.Administration.ViewModels;
using Get_A_Taxi.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Get_A_Taxi.Web.Infrastructure.Services.Contracts
{
    public interface IAccountService
    {
        ICollection<UserItemViewModel> GetAccounts();
        ICollection<UserItemViewModel> GetAccountsByRole(UserRoles role, ApplicationRoleManager roleManager);
        ICollection<UserItemViewModel> GetAccountsByTextSearch(string textToSerach);

        ICollection<UserItemViewModel> GetAccountByDistrict(string districtText);
        ICollection<UserItemViewModel> GetAccountsByRoleAndDistrict(int districtId, UserRoles role, ApplicationRoleManager roleManager);
        ICollection<UserItemViewModel> GetAccountsByRoleAndDistrict(string districtText, string roleTextSearch, ApplicationRoleManager roleManager);
    }
}
