using Get_A_Taxi.Data;
using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Get_A_Taxi.Web.Infrastructure.Services.HubServices
{
    public class OrdersHubService : BaseService, IOrdersHubService
    {
        public OrdersHubService(IGetATaxiData data)
            :base(data)
        {
        }

        public IQueryable<Order> AllOrders()
        {
            return this.Data
                .Orders.All();
        }

        public IQueryable<Order> GetUnfinished(IQueryable<Order> orders)
        {
            return orders.Where(o => o.OrderStatus != OrderStatus.Finished && o.OrderStatus != OrderStatus.Cancelled);
        }


        public IQueryable<Order> ByDistrict(IQueryable<Order> orders, int districtId)
        {
            return orders.Where(o => o.District.DistrictId == districtId);
        }
    }
}