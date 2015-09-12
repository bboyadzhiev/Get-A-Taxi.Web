using Get_A_Taxi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Get_A_Taxi.Web.Infrastructure.Services.HubServices
{
    public interface IOrdersHubService
    {
        IQueryable<Order> AllOrders();
        IQueryable<Order> ByDistrict(IQueryable<Order> orders, int districtId);
        IQueryable<Order> GetUnfinished(IQueryable<Order> orders);
    }
}
