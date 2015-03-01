using Get_A_Taxi.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Get_A_Taxi.Web.Infrastructure.Services.Hubs
{
    public interface IOrderBridge
    {
        event EventHandler<OrderBidgeEventArgs> OrderAddedEvent;
        event EventHandler<OrderBidgeEventArgs> OrderCancelledEvent;
        event EventHandler<OrderBidgeEventArgs> OrderUpdatedEvent;

        void AddOrder(OrderDetailsVM order, int districtId);
        void CancelOrder(OrderDetailsVM order, int districtId);
        void UpdateOrder(OrderDetailsVM order, int districtId);
    }
}
