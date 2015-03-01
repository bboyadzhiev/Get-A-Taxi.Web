namespace Get_A_Taxi.Web.Infrastructure.Services.Contracts
{
    using Get_A_Taxi.Models;
    using System.Collections.Generic;
    using System.Linq;

    public interface IOrdersService
    {
        IQueryable<Order> AllOrders();
        IQueryable<Order> ByStatus(IQueryable<Order> orders, OrderStatus status);
        IQueryable<Order> ByDistrict(IQueryable<Order> orders, int districtId);
        IQueryable<Order> ByTaxi(IQueryable<Order> orders, int taxiId);
        IQueryable<Order> GetUnfinished(IQueryable<Order> orders);
    }
}
