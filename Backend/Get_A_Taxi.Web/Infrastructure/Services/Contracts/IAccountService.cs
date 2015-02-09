namespace Get_A_Taxi.Web.Infrastructure.Services
{
    using Get_A_Taxi.Models;
    using System.Linq;

    public interface IAccountService
    {
        IQueryable<ApplicationUser> AllUsers();
        IQueryable<ApplicationUser> GetEmployees(IQueryable<ApplicationUser> users);
        IQueryable<ApplicationUser> WithDistrictLike(IQueryable<ApplicationUser> users, string districtName);
        IQueryable<ApplicationUser> WithFirstNameLike(IQueryable<ApplicationUser> users, string nameString);
        IQueryable<ApplicationUser> WithFullNameContaining(IQueryable<ApplicationUser> users, string nameString);
        IQueryable<ApplicationUser> WithLastNameLike(IQueryable<ApplicationUser> users, string nameString);
        IQueryable<ApplicationUser> WithMiddletNameLike(IQueryable<ApplicationUser> users, string nameString);
        IQueryable<ApplicationUser> WithRole(IQueryable<ApplicationUser> users, string roleId);
    }
}
