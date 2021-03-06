﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
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

namespace Get_A_Taxi.Web.WebAPI
{
    /// <summary>
    /// Manages orders for the clients of the system.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/ClientOrders")]
    public class ClientOrdersController : BaseApiController, IRESTController<OrderDetailsDTO>
    {
        private const int RESULTS_COUNT = 10;
        private IOrderBridge bridge;
        public ClientOrdersController(IGetATaxiData data, IOrderBridge bridge)
            : base(data)
        {
            this.bridge = bridge;
        }

        /// <summary>
        /// Get last unfinished order for this client
        /// </summary>
        /// <returns>The latest found unfinished order or empty result</returns>
        [HttpGet]
        public IHttpActionResult Get()
        {
            var user = GetUser();
            var order = this.Data.Orders.All()
                .Where(o => o.Customer.Id == user.Id
                    && (o.OrderStatus != OrderStatus.Cancelled || o.OrderStatus != OrderStatus.Finished))
                .OrderByDescending(o => o.OrderedAt)
                .FirstOrDefault();

            if (order != null)
            {
                var result = Mapper.Map<OrderDetailsDTO>(order);
                return Ok(result);
            }

            return Ok();
        }

        /// <summary>
        /// Get the details of a placed order in the system
        /// </summary>
        /// <param name="id">Order's id</param>
        /// <returns>Orders's model with included taxi details</returns>
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var user = GetUser();
            var order = this.Data.Orders
                .SearchFor(o => o.OrderId == id && o.Customer.Id == user.Id)
                .FirstOrDefault();

            if (order == null)
            {
                return NotFound();
            }

            var orderDTO = Mapper.Map<Order, OrderDetailsDTO>(order);

            return Ok(orderDTO);
        }

        /// <summary>
        /// Get a page of the client's orders
        /// </summary>
        /// <param name="page">Page number</param>
        /// <returns>List of data models of client orders</returns>
        [HttpGet]
        public IHttpActionResult GetPaged(int page)
        {
            var user = GetUser();
            var orders = this.Data.Orders.All().Where(o => o.Customer.Id == user.Id)
                .Skip(page * RESULTS_COUNT)
                .Take(RESULTS_COUNT)
                .Project().To<OrderDTO>().ToList();
            return Ok(orders);
        }

        /// <summary>
        /// Creates a new client order. The district for it will be the closest one in the system.
        /// </summary>
        /// <param name="model">The data model of the new order</param>
        /// <returns>The data model of the newly created order</returns>
        [HttpPost]
        public IHttpActionResult Post([FromBody]OrderDetailsDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model state!");
            }

            var customer = GetUser();

            // Checking for unfinished orders for this customer
            var order = this.Data.Orders.All()
                .Where(o => o.Customer.Id == customer.Id && (o.OrderStatus == OrderStatus.Waiting || o.OrderStatus == OrderStatus.InProgress))
                .FirstOrDefault();

            // Finds the closes district
            // HACK: Review distance calculation
            var closestDistrict = this.Data.Districts.All().OrderBy(d => (Math.Abs(d.CenterLatitude - model.OrderLatitude) + Math.Abs(d.CenterLongitude - model.OrderLongitude))).FirstOrDefault();
            if (order != null)
            {

                var newStatus = order.OrderStatus;
                // Previous order is not finished, updating with new details and returning new model
                order = UpdateOrderFromModel(model, customer, order, closestDistrict, newStatus);

                this.Data.Orders.Update(order);
                this.Data.Orders.SaveChanges();

                var updatedOrder = this.Data.Orders.SearchFor(o => o.OrderId == order.OrderId).FirstOrDefault();
                var updatedOrderModel = Mapper.Map<OrderDetailsDTO>(updatedOrder);
                this.bridge.UpdateOrder(updatedOrderModel, updatedOrder.District.DistrictId);

                return Ok(updatedOrderModel);
            }
            
            // New order
            order = OrderDetailsDTO.ToOrderModel(model, customer);

            order.Customer = customer;
            order.District = closestDistrict;

            this.Data.Orders.Add(order);
            this.Data.Orders.SaveChanges();

            var addedOrder = this.Data.Orders.SearchFor(o => o.OrderId == order.OrderId).FirstOrDefault();

            var addedOrderModel = Mapper.Map<OrderDetailsDTO>(addedOrder);
            this.bridge.AddOrder(addedOrderModel, addedOrder.District.DistrictId);

            return Ok(addedOrderModel);
        }

       

        /// <summary>
        /// Updates a client order. Possible only if it is still in "Unassigned" or "Waiting" state.
        /// </summary>
        /// <param name="model">The updated order's data model</param>
        /// <returns>The updated order's data model</returns>
        public IHttpActionResult Put([FromBody]OrderDetailsDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model state!");
            }

            var orderToUpdate = this.Data.Orders.SearchFor(o => o.OrderId == model.OrderId && o.OrderStatus < OrderStatus.Finished).FirstOrDefault();

            if (orderToUpdate == null)
            {
                return NotFound();
            }

            var customer = this.GetUser();

            if (orderToUpdate.Customer.Id != customer.Id)
            {
                return BadRequest("This is not your order!");
            }

            if (orderToUpdate.OrderStatus != OrderStatus.Waiting && orderToUpdate.OrderStatus != OrderStatus.Unassigned )
            {
                return BadRequest("Order can not be changed by customer after pickup!");
            }

            var district = orderToUpdate.District;

            orderToUpdate = UpdateOrderFromModel(model, customer, orderToUpdate, district, orderToUpdate.OrderStatus);

            this.Data.Orders.Update(orderToUpdate);
            this.Data.Orders.SaveChanges();

            var updatedOrder = this.Data.Orders.SearchFor(o => o.OrderId == orderToUpdate.OrderId).FirstOrDefault();
            var updatedOrderModel = Mapper.Map<OrderDetailsDTO>(updatedOrder);
            this.bridge.UpdateOrder(updatedOrderModel, updatedOrder.District.DistrictId);

            return Ok(updatedOrderModel);

        }

        /// <summary>
        /// Cancels a client order. 
        /// Possible only if it is still in "Unassigned" or "Waiting" state.
        /// </summary>
        /// <param name="id">The id of the client order</param>
        /// <returns>The cancelled order's data model</returns>
        public IHttpActionResult Delete(int id)
        {
            var orderToCancel = this.Data.Orders.SearchFor(o => o.OrderId == id).FirstOrDefault();
            if (orderToCancel == null)
            {
                return NotFound();
            }

            var customer = this.GetUser();
            if (orderToCancel.Customer.Id != customer.Id)
            {
                return BadRequest("This is not your order!");
            }

            if (orderToCancel.OrderStatus == OrderStatus.Unassigned || orderToCancel.OrderStatus == OrderStatus.Waiting)
            {
                orderToCancel.OrderStatus = OrderStatus.Cancelled;
                var district = orderToCancel.District;
                
                this.Data.Orders.Update(orderToCancel);
                this.Data.SaveChanges();

                this.bridge.CancelOrder(orderToCancel.OrderId, orderToCancel.District.DistrictId);

                var orderDM = Mapper.Map<Order, OrderDTO>(orderToCancel);

                return Ok(orderDM);
            }
            return Conflict();
            //return BadRequest("Order can be cancelled only if it is unassigned or waiting!");
        }

        #region Helpers
        [NonAction]
        private static Order UpdateOrderFromModel(OrderDetailsDTO model, ApplicationUser customer, Order order, District closestDistrict, OrderStatus newStatus)
        {
            order.OrderLatitude = model.OrderLatitude;
            order.OrderLongitude = model.OrderLongitude;
            order.OrderAddress = model.OrderAddress;

            if (!string.IsNullOrEmpty(model.UserComment))
            {
                order.UserComment = model.UserComment;
            }

            if (!string.IsNullOrEmpty(model.DestinationAddress))
            {
                order.DestinationAddress = model.DestinationAddress;
                order.DestinationLatitude = model.DestinationLatitude;
                order.DestinationLongitude = model.DestinationLongitude;
            }

            order.Customer = customer;
            order.District = closestDistrict;
            order.OrderStatus = newStatus;
            return order;
        }
        #endregion
    }
}
