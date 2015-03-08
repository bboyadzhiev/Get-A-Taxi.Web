namespace Get_A_Taxi.Web.Infrastructure.Services
{
    using System.Collections.Generic;
    using System.Linq;
   
    using Get_A_Taxi.Data;
    using Get_A_Taxi.Models;
    using Get_A_Taxi.Web.Infrastructure.Services.Base;
    using Get_A_Taxi.Web.Infrastructure.Services.Contracts;

    public class OrdersService: BaseService, IOrdersService
    {
        public OrdersService(IGetATaxiData data)
            :base(data)
        {
        }

        public IQueryable<Order> AllOrders()
        {
            return this.Data
                .Orders.All();
        }

        public IQueryable<Order> ByStatus(IQueryable<Order> orders, OrderStatus status)
        {
            return orders.Where(o => o.OrderStatus == status);
        }

        public IQueryable<Order> ByDistrict(IQueryable<Order> orders, int districtId)
        {
            return orders.Where(o => o.District.DistrictId == districtId);
        }

        public IQueryable<Order> ByTaxi(IQueryable<Order> orders, int taxiId)
        {
            return orders.Where(o => o.AssignedTaxi.TaxiId == taxiId);
        }

        public IQueryable<Order> GetUnfinished(IQueryable<Order> orders)
        {
            return orders.Where(o => o.OrderStatus != OrderStatus.Finished && o.OrderStatus != OrderStatus.Cancelled);
        }
    }
}