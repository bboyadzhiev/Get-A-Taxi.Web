using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;
using Get_A_Taxi.Web.Infrastructure.Services.HubServices;
using Get_A_Taxi.Web.Models;

namespace Get_A_Taxi.Web.Infrastructure.Bridges
{

    public class OrderBridge : BaseBridge, IOrderBridge
    {
        private event EventHandler<OrderBridgeDetailedEventArgs> OrderAddedEvent;
        private event EventHandler<OrderBridgeEventArgs> OrderCancelledEvent;
        private event EventHandler<OrderBridgeAssignmentEventArgs> OrderAssignedEvent;
        private event EventHandler<OrderBridgeDetailedEventArgs> OrderUpdatedEvent;

        public void AddOrder(OrderDetailsDTO order, int districtId)
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

        public void UpdateOrder(OrderDetailsDTO order, int districtId)
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
                Clients.Group(districtGroup).updatedOrder(e.order);
            };

            OrderAddedEvent += orderAddedHandler;

            OrderCancelledEvent += orderCancelledHandler;

            OrderAssignedEvent += orderAssignedHandler;

            OrderUpdatedEvent += orderUpdatedHandler;
        }

    }
}