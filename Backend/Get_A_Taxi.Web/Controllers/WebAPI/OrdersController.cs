using AutoMapper;
using Get_A_Taxi.Data;
using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.Bridges;
using Get_A_Taxi.Web.Models;
using Get_A_Taxi.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Get_A_Taxi.Web.Controllers.WebAPI
{
    [Authorize]
    public class OrdersController : BaseApiController, IRESTController<OrderDataModel>
    {
        private IOrderBridge bridge;
        public OrdersController(IGetATaxiData data, IOrderBridge bridge)
            : base(data)
        {
            this.bridge = bridge;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            var user = GetUser();
            var orders = this.Data.Orders.SearchFor(o => o.Customer.Id == user.Id).ToList();
            return Ok(orders);
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var order = this.Data.Orders.SearchFor(o => o.OrderId == id).FirstOrDefault();
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody]OrderDataModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model state!");
            }

            var newOrder = Mapper.Map<Order>(model);

            var closestDistrict = this.Data.Districts.All().OrderBy(d => ((d.CenterLattitude - model.OrderLattitude) + (d.CenterLongitude - model.OrderLongitude))).FirstOrDefault();
            var user = GetUser();

            newOrder.District = closestDistrict;
            newOrder.Customer = user;
            newOrder.OrderStatus = OrderStatus.Waiting;

            this.Data.Orders.Add(newOrder);
            this.Data.Orders.SaveChanges();

            var addedOrder = this.Data.Orders.SearchFor(o => o.OrderId == newOrder.OrderId).FirstOrDefault();
            var addedOrderModel = Mapper.Map<OrderDataModel>(addedOrder);
            
            var orderVM = Mapper.Map<OrderDetailsVM>(addedOrder);
            this.bridge.AddOrder(orderVM, addedOrder.District.DistrictId);
            
            return Ok(addedOrderModel);
        }

        public IHttpActionResult Put([FromBody]OrderDataModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model state!");
            }

            var orderToUpdate = this.Data.Orders.SearchFor(o => o.OrderId == model.OrderId).FirstOrDefault() ;

            if (orderToUpdate == null)
            {
                return NotFound();
            }

            if (orderToUpdate.OrderStatus != OrderStatus.Waiting)
            {
                return BadRequest("Order can be changed only in waiting state!");
            }

            orderToUpdate = Mapper.Map<Order>(model);
            this.Data.Orders.Update(orderToUpdate);
            this.Data.Orders.SaveChanges();
            
            var orderVM = Mapper.Map<OrderDetailsVM>(orderToUpdate);
            this.bridge.UpdateOrder(orderVM, orderToUpdate.District.DistrictId);

            return Ok(model);

        }

        public IHttpActionResult Delete(int id)
        {
            var orderToCancel = this.Data.Orders.SearchFor(o => o.OrderId == id).FirstOrDefault();
            if (orderToCancel == null)
            {
                return NotFound();
            }

            if (orderToCancel.OrderStatus != OrderStatus.Waiting)
            {
                return BadRequest("Order can be changed only in waiting state!");
            }

            orderToCancel.OrderStatus = OrderStatus.Cancelled;
            var orderToCancelModel = Mapper.Map<OrderDetailsVM>(orderToCancel);
            this.Data.Orders.Update(orderToCancel);
            this.Data.SaveChanges();

            this.bridge.CancelOrder(orderToCancelModel, orderToCancel.District.DistrictId);

            return Ok(orderToCancel);
        }
    }
}
