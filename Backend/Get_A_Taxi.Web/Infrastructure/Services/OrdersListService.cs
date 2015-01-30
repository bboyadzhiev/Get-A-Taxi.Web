namespace Get_A_Taxi.Web.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Get_A_Taxi.Data;
    using Get_A_Taxi.Models;
    using Get_A_Taxi.Web.Infrastructure.Services.Base;
    using Get_A_Taxi.Web.Infrastructure.Services.Contracts;
    using Get_A_Taxi.Web.ViewModels;

    public class OrdersListService: BaseService, IOrdersListService
    {
        public OrdersListService(IGetATaxiData data)
            :base(data)
        {
        }

        public IList<OrderViewModel> GetOrderVMList(int count)
        {
            var ordersListVM = this.Data
                .Orders.All()
                .Where(o => o.OrderStatus != OrderStatus.Finished)
                .OrderByDescending(o => o.OrderedAt)
                .Take(count)
                .Select(OrderViewModel.FromOrderDataModel)
                .ToList();

            return ordersListVM;
        }
    }
}