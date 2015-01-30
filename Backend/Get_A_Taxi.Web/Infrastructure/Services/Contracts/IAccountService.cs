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
        ICollection<AccountItemViewModel> GetAccounts();
        ICollection<AccountItemViewModel> GetAccountsByRole(UserRoles role, ApplicationRoleManager roleManager);
        ICollection<AccountItemViewModel> GetAccountsByTextSearch(string textToSerach);

        ICollection<AccountItemViewModel> GetAccountByDistrict(string districtText);
        ICollection<AccountItemViewModel> GetAccountsByRoleAndDistrict(int districtId, UserRoles role, ApplicationRoleManager roleManager);
        ICollection<AccountItemViewModel> GetAccountsByRoleAndDistrict(string districtText, string roleTextSearch, ApplicationRoleManager roleManager);
    }
}
