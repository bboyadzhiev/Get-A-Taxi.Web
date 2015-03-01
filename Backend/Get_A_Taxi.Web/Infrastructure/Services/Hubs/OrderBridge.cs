using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Get_A_Taxi.Web.ViewModels;
using Get_A_Taxi.Web.Infrastructure.Services.Hubs.HubServices;
using Microsoft.AspNet.SignalR.Hubs;

namespace Get_A_Taxi.Web.Infrastructure.Services.Hubs
{
    public class OrderBidgeEventArgs : EventArgs
    {
        //   public int orderId { get; set; }
        public OrderDetailsVM order { get; set; }
        public int districtId { get; set; }
    }

    public class OrderBridge : IOrderBridge
    {
        public event EventHandler<OrderBidgeEventArgs> OrderAddedEvent;
        public event EventHandler<OrderBidgeEventArgs> OrderCancelledEvent;
        public event EventHandler<OrderBidgeEventArgs> OrderUpdatedEvent;

        public void AddOrder(OrderDetailsVM order, int districtId)
        {
            if (OrderAddedEvent != null)
            {
                var args = new OrderBidgeEventArgs() { districtId = districtId, order = order };
                OrderAddedEvent(this, args);
            }
        }

        public void CancelOrder(OrderDetailsVM order, int districtId)
        {
            if (OrderCancelledEvent != null)
            {
                var args = new OrderBidgeEventArgs() { districtId = districtId, order = order };
                OrderCancelledEvent(this, args);
            }
        }

        public void UpdateOrder(OrderDetailsVM order, int districtId)
        {
            if (OrderUpdatedEvent != null)
            {
                var args = new OrderBidgeEventArgs() { districtId = districtId, order = order };
                OrderUpdatedEvent(this, args);
            }
        }

        private EventHandler<OrderBidgeEventArgs> orderAddedHandler;
        private EventHandler<OrderBidgeEventArgs> orderUpdatedHandler;
        private EventHandler<OrderBidgeEventArgs> orderCancelledHandler;

        private IHubConnectionContext<dynamic> Clients { get; set; }
        public OrderBridge(IOrdersHubService service, IHubConnectionContext<dynamic> clients)
        {
            if (clients == null)
            {
                throw new ArgumentNullException("clients");
            }

            Clients = clients;
            orderAddedHandler = (s, e) =>
             {
                 var districtGroup = e.districtId.ToString();
                 //var orderVM = this._service.AllOrders().Where(o => o.OrderId == e.orderId).Project().To<OrderDetailsVM>().FirstOrDefault();
                 Clients.Group(districtGroup).addedOrder(e.order);
             };

            orderUpdatedHandler = (s, e) =>
            {
                var districtGroup = e.districtId.ToString();
                //var orderVM = this._service.AllOrders().Where(o => o.OrderId == e.orderId).Project().To<OrderDetailsVM>().FirstOrDefault();
                Clients.Group(districtGroup).updatedOrder(e.order);
            };

            orderCancelledHandler = (s, e) =>
            {
                var districtGroup = e.districtId.ToString();
                Clients.Group(districtGroup).cancelledOrder(e.order);
            };

            OrderAddedEvent += orderAddedHandler;

            OrderUpdatedEvent += orderUpdatedHandler;

            OrderCancelledEvent += orderCancelledHandler;
        }

    }
}