using Get_A_Taxi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Get_A_Taxi.Web.Infrastructure.Services.Hubs.HubServices
{
    public interface IOrdersHubService
    {
        IQueryable<Order> AllOrders();
        IQueryable<Order> GetUnfinished(IQueryable<Order> orders);
    }
}
