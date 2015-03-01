namespace Get_A_Taxi.Web.Infrastructure.Populators
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface IDropDownListPopulator
    {
        IEnumerable<SelectListItem> GetDistricts();
        IEnumerable<SelectListItem> GetNullableDistricts();
        IEnumerable<SelectListItem> GetRoles(ApplicationRoleManager manager);

        IEnumerable<SelectListItem> GetRolesForManagement(ApplicationRoleManager manager);

        void clearCache(string cacheId);
        void clearDistrictCaches();
    }
}
