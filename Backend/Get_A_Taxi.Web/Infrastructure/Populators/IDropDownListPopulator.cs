namespace Get_A_Taxi.Web.Infrastructure.Populators
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface IDropDownListPopulator
    {
        /// <summary>
        /// Get selectlist of all available districts
        /// </summary>
        /// <returns>Collection of SelectList districts</returns>
        IEnumerable<SelectListItem> GetDistricts();

        /// <summary>
        /// Get selectlist of all available districts 
        /// and one marked as "----", used for null-district search
        /// </summary>
        /// <returns>Collection of SelectList districts</returns>
        IEnumerable<SelectListItem> GetNullableDistricts();

        /// <summary>
        /// Get selectlist of all available roles
        /// </summary>
        /// <param name="manager">Role manager</param>
        /// <returns></returns>
        IEnumerable<SelectListItem> GetRoles(ApplicationRoleManager manager);

        IEnumerable<SelectListItem> GetRolesForManagement(ApplicationRoleManager manager);

        void clearCache(string cacheId);
        void clearDistrictCaches();
    }
}
