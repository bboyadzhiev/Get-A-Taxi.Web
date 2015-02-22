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
using Get_A_Taxi.Web.Infrastructure.Services.Contracts;

namespace Get_A_Taxi.Web.Hubs
{
    [HubName("ordersHub")]
    public class OrdersHub : Hub
    {
        private IOrderBridge _ordersBridge;
        private IOrdersService _service;
       // private IGetATaxiData _data;
        public OrdersHub(IOrderBridge ordersBridge, IOrdersService service)
        {
            this._ordersBridge = ordersBridge;
            this._service = service;
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
            //{
            //    var orderVM = this.data.Orders.All().Where(o => o.OrderId == e).Select(OrderDetailsVM.FromOrderDataModel).FirstOrDefault();
            //    Clients.All.addedOrder(orderVM);
            //};

            _ordersBridge.OrderAddedEvent += (s, e) =>
            {
                var orderVM = this._service.AllOrders().Where(o => o.OrderId == e).Project().To<OrderDetailsVM>().FirstOrDefault();
                Clients.All.addedOrder(orderVM);
            };

            _ordersBridge.OrderUpdatedEvent += (s, e) =>
            {
                var orderVM = this._service.AllOrders().Where(o => o.OrderId == e).Project().To<OrderDetailsVM>().FirstOrDefault();
                Clients.All.updatedOrder(orderVM);
            };

            _ordersBridge.OrderCancelledEvent += (s, e) => Clients.All.cancelledOrder(e);

            var result = this._service.AllOrders();
            result = this._service.GetUnfinished(result);
            var ordersDisplayVMList = result.Project().To<OrderDetailsVM>().ToList();
            Clients.Caller.updateOrders(ordersDisplayVMList);

        }
    }
}