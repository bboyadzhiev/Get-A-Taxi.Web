namespace Get_A_Taxi.Web.Infrastructure.Services
{
    using System.Linq;
    
    using Get_A_Taxi.Data;
    using Get_A_Taxi.Models;
    using Get_A_Taxi.Web.Infrastructure.Services.Base;
    using Get_A_Taxi.Web.Infrastructure.Services.Contracts;
    using Get_A_Taxi.Web.ViewModels;
    using System;

    public class OperatorService : BaseService, IOperatorService
    {
        public OperatorService(IGetATaxiData data)
            :base(data)
        {
        }

        public IQueryable<Order> OrdersByWaitingTime()
        {
            return this.Data.Orders.All()
                 .Where(o => o.OrderStatus == OrderStatus.Waiting)
                 .OrderByDescending(o => o.OrderedAt);
        }

        public IQueryable<Order> OrdersByCompletionTime()
        {
            return this.Data.Orders.All()
                .Where(o => o.OrderStatus == OrderStatus.InProgress)
                .OrderByDescending(o => o.ArrivalTime);
        }

        public IQueryable<Order> OrdersByCompletionNearLocation(double lat, double lon)
        {
            return this.Data.Orders.All()
                .Where(o => o.OrderStatus == OrderStatus.InProgress)
                .OrderBy(o => ((o.OrderLattitude-lat) + (o.OrderLongitude - lon)));
        }
    }
}