using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Get_A_Taxi.Web.ViewModels;
using Get_A_Taxi.Web.Infrastructure.Services.Hubs.HubServices;
using Microsoft.AspNet.SignalR.Hubs;

namespace Get_A_Taxi.Web.Infrastructure.Bridges
{


    public class OrderBridge : BaseBridge, IOrderBridge
    {
        public event EventHandler<OrderBridgeDetailedEventArgs> OrderAddedEvent;
        public event EventHandler<OrderBridgeEventArgs> OrderCancelledEvent;
        public event EventHandler<OrderBridgeAssignmentEventArgs> OrderAssignedEvent;
        public event EventHandler<OrderBridgeDetailedEventArgs> OrderUpdatedEvent;

        public void AddOrder(OrderDetailsVM order, int districtId)
        {
            if (OrderAddedEvent != null)
            {
                var args = new OrderBridgeDetailedEventArgs() { districtId = districtId, order = order };
                OrderAddedEvent(this, args);
            }
        }

        public void CancelOrder(int orderId, int districtId)
        {
            if (OrderCancelledEvent != null)
            {
                var args = new OrderBridgeEventArgs() { districtId = districtId, orderId = orderId };
                OrderCancelledEvent(this, args);
            }
        }

        public void AssignOrder(int orderId, int taxiId, int districtId)
        {
            if (OrderAssignedEvent != null)
            {
                var args = new OrderBridgeAssignmentEventArgs() { districtId = districtId, orderId = orderId, taxiId = taxiId };
                OrderAssignedEvent(this, args);
            }
        }

        public void UpdateOrder(OrderDetailsVM order, int districtId)
        {
            if (OrderUpdatedEvent != null)
            {
                var args = new OrderBridgeDetailedEventArgs() { districtId = districtId, order = order };
                OrderUpdatedEvent(this, args);
            }
        }

        private EventHandler<OrderBridgeDetailedEventArgs> orderAddedHandler;
        private EventHandler<OrderBridgeEventArgs> orderCancelledHandler;
        private EventHandler<OrderBridgeAssignmentEventArgs> orderAssignedHandler;
        private EventHandler<OrderBridgeDetailedEventArgs> orderUpdatedHandler;

        public OrderBridge(IHubConnectionContext<dynamic> clients)
            : base(clients)
        {

            orderAddedHandler = (s, e) =>
             {
                 var districtGroup = e.districtId.ToString();
                 //var orderVM = this._service.AllOrders().Where(o => o.OrderId == e.orderId).Project().To<OrderDetailsVM>().FirstOrDefault();
                 Clients.Group(districtGroup).addedOrder(e.order);
             };


            orderCancelledHandler = (s, e) =>
            {
                var districtGroup = e.districtId.ToString();
                Clients.Group(districtGroup).cancelledOrder(e.orderId);
            };

            // Notify all about order assignment
            orderAssignedHandler = (s, e) =>
            {
                var districtGroup = e.districtId.ToString();
                Clients.Group(districtGroup).assignedOrder(e.orderId, e.taxiId);
            };

            orderUpdatedHandler = (s, e) =>
            {
                var districtGroup = e.districtId.ToString();
                //var orderVM = this._service.AllOrders().Where(o => o.OrderId == e.orderId).Project().To<OrderDetailsVM>().FirstOrDefault();
                Clients.Group(districtGroup).updatedOrder(e.order);
            };

            OrderAddedEvent += orderAddedHandler;

            OrderCancelledEvent += orderCancelledHandler;

            OrderAssignedEvent += orderAssignedHandler;

            OrderUpdatedEvent += orderUpdatedHandler;
        }




    }
}