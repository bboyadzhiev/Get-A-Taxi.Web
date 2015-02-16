using Get_A_Taxi.Web.Infrastructure.Services.Hubs;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;
using Get_A_Taxi.Web.Infrastructure;

namespace Get_A_Taxi.Web.Hubs
{
    [HubName("ordersHub")]
    public class OrdersHub : Hub
    {
        //private IOrderBridge _ordersBridge;
        //public OrdersHub(IOrderBridge ordersBridge)
        //{
        //    this._ordersBridge = ordersBridge;
        //}
        ////public OrdersHub()
        ////    :this(new OrderBridge())
        ////{

        ////}

        //public void Start()
        //{
        //    _ordersBridge.OrderAddedEvent += (s, e) => Clients.All.addedOrder(e);
        //    _ordersBridge.OrderCancelledEvent += (s, e) => Clients.All.cancelledOrder(e);

        //}

        public void Start()
        {
            OrdersEvents.OrderAddedEvent += (s, e) => Clients.All.addedOrder(e);
        }
    }
}