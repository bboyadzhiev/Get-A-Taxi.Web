using Get_A_Taxi.Data;
using Get_A_Taxi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Get_A_Taxi.Web.Infrastructure.Services.Hubs.HubServices
{
    public class OrdersHubService : IOrdersHubService
    {
        protected IGetATaxiData Data { get; private set; }
        public OrdersHubService(IGetATaxiData data)
        {
            this.Data = data;
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