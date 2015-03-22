using Get_A_Taxi.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Get_A_Taxi.Web.Infrastructure.Bridges
{
    public interface IOrderBridge
    {
        event EventHandler<OrderBridgeDetailedEventArgs> OrderAddedEvent;
        event EventHandler<OrderBridgeEventArgs> OrderCancelledEvent;
        event EventHandler<OrderBridgeAssignmentEventArgs> OrderAssignedEvent;
        event EventHandler<OrderBridgeDetailedEventArgs> OrderUpdatedEvent;

        void AddOrder(OrderDetailsVM order, int districtId);
        void CancelOrder(int orderId, int districtId);
        void AssignOrder(int orderId, int taxiId, int districtId);
        void UpdateOrder(OrderDetailsVM order, int districtId);

    }
}
