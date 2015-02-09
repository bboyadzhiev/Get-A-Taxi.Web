namespace Get_A_Taxi.Web.Infrastructure.Populators
{
    using Get_A_Taxi.Data;
    using Get_A_Taxi.Models;
    using Get_A_Taxi.Web.Infrastructure.Caching;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class DropDownListPopulator : IDropDownListPopulator
    {
        private IGetATaxiData data;
        private ICacheService cache;

        public DropDownListPopulator(IGetATaxiData data, ICacheService cache)
        {
            this.data = data;
            this.cache = cache;
        }

        public IEnumerable<SelectListItem> GetDistricts()
        {

            var districts = this.cache.Get<IEnumerable<SelectListItem>>("districts",
                () =>
                {
                    return this.data.Districts.All()
                        .Select(d => new SelectListItem
                        {
                            Value = d.DistrictId.ToString(),
                            Text = d.Title
                        })
                        .ToList();
                });

            return districts;
        }

        public IEnumerable<SelectListItem> GetRoles(ApplicationRoleManager manager)
        {
            var cachedRoles = this.cache.Get<IEnumerable<SelectListItem>>("roles",
                () =>
                {
                    var roles = manager.Roles.ToList();
                    List<SelectListItem> roleItems = new List<SelectListItem>();

                    foreach (var role in roles)
                    {
                        roleItems.Add(new SelectListItem
                        {
                            Text = role.Name,
                            Value = role.Id
                        });
                    }
                    return roleItems;
                });
            return cachedRoles;
        }


        public IEnumerable<SelectListItem> GetRolesForManagement(ApplicationRoleManager manager)
        {
            var cachedRoles = this.cache.Get<IEnumerable<SelectListItem>>("roles",
               () =>
               {
                   var roles = manager.Roles.ToList();
                   List<SelectListItem> roleItems = new List<SelectListItem>();

                   foreach (var role in roles)
                   {
                       if (role.Name == UserRoles.Driver.ToString() || role.Name == UserRoles.Operator.ToString())
                       {
                           roleItems.Add(new SelectListItem
                           {
                               Text = role.Name,
                               Value = role.Id
                           });
                       }
                   }
                   return roleItems;
               });
            return cachedRoles;
        }
    }
}