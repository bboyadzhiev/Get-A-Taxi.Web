using Get_A_Taxi.Web.Infrastructure.Services.Hubs;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR.Hubs;
using Get_A_Taxi.Web.Infrastructure;
using Get_A_Taxi.Data;
using Get_A_Taxi.Models;
using Get_A_Taxi.Web.ViewModels;

using AutoMapper.QueryableExtensions;

namespace Get_A_Taxi.Web.Hubs
{
    [HubName("ordersHub")]
    public class OrdersHub : Hub
    {
        private IOrderBridge _ordersBridge;
        private IGetATaxiData _data;
        public OrdersHub(IOrderBridge ordersBridge, IGetATaxiData data)
        {
            this._ordersBridge = ordersBridge;
            this._data = data;
        }

        //public OrdersHub(IGetATaxiData data)
        //{
        //    this.data = data;
        //}

        public void Start()
        {
            // TODO : OrdersBridge has bugs - sends too many callbacks to client
            // Most likely dependency injection issue

            // Fallback to static event dispathcher
            //  OrdersEvents.OrderAddedEvent += (s, e) =>
            _ordersBridge.OrderAddedEvent += (s, e) =>
           {
               var orderVM = this._data.Orders.All().Where(o => o.OrderId == e).Project().To<OrderDetailsVM>().FirstOrDefault();
               // var orderVM = this.data.Orders.All().Where(o => o.OrderId == e).Select(OrderDetailsVM.FromOrderDataModel).FirstOrDefault();
               Clients.All.addedOrder(orderVM);
           };

            //// OrdersEvents.OrderCancelledEvent += (s, e) => Clients.All.cancelledOrder(e);
            var ordersDisplayVMList = this._data.Orders.All().Where(o => o.OrderStatus != OrderStatus.Finished).Project().To<OrderDetailsVM>().ToList();
            Clients.Caller.updateOrders(ordersDisplayVMList);
        }
    }
}