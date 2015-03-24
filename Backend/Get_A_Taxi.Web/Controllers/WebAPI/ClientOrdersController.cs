using AutoMapper;
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

namespace Get_A_Taxi.Web.Controllers.WebAPI
{
    /// <summary>
    /// Manages orders for the clients of the system.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/ClientOrders")]
    public class ClientOrdersController : BaseApiController, IRESTController<OrderDTO>
    {
        private const int RESULTS_COUNT = 10;
        private IOrderBridge bridge;
        public ClientOrdersController(IGetATaxiData data, IOrderBridge bridge)
            : base(data)
        {
            this.bridge = bridge;
        }

        /// <summary>
        /// Get all client orders
        /// </summary>
        /// <returns>List of data models of client orders</returns>
        [HttpGet]
        public IHttpActionResult Get()
        {
            var user = GetUser();
            var orders = this.Data.Orders.All().Where(o => o.Customer.Id == user.Id)
                .Project().To<OrderDTO>().ToList();
            return Ok(orders);
        }

        /// <summary>
        /// Get client order by id
        /// </summary>
        /// <param name="id">Order's id</param>
        /// <returns>List of data models of client orders</returns>
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var user = GetUser();
            var orders = this.Data.Orders
                .SearchFor(o => o.OrderId == id && o.Customer.Id == user.Id)
                .Project().To<OrderDTO>().ToList();

            return Ok(orders);
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
        /// <returns>The data model of the new order</returns>
        [HttpPost]
        public IHttpActionResult Post([FromBody]OrderDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model state!");
            }

            var newOrder = Mapper.Map<Order>(model);

            // TODO: Review distance calculation
            // Finds the closes district
            var closestDistrict = this.Data.Districts.All().OrderBy(d => ((d.CenterLattitude - model.OrderLattitude) + (d.CenterLongitude - model.OrderLongitude))).FirstOrDefault();
            var user = GetUser();

            newOrder.District = closestDistrict;
            newOrder.Customer = user;
            newOrder.OrderStatus = OrderStatus.Waiting;

            this.Data.Orders.Add(newOrder);
            this.Data.Orders.SaveChanges();

            var addedOrder = this.Data.Orders.SearchFor(o => o.OrderId == newOrder.OrderId).FirstOrDefault();
            var addedOrderModel = Mapper.Map<OrderDTO>(addedOrder);

            var orderVM = Mapper.Map<OrderDetailsVM>(addedOrder);
            this.bridge.AddOrder(orderVM, addedOrder.District.DistrictId);

            return Ok(addedOrderModel);
        }

        /// <summary>
        /// Updates a client order. Possible only if it's in the "Waiting" state.
        /// </summary>
        /// <param name="model">The updated order's data model</param>
        /// <returns>The updated order's data model</returns>
        public IHttpActionResult Put([FromBody]OrderDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model state!");
            }

            var orderToUpdate = this.Data.Orders.SearchFor(o => o.OrderId == model.OrderId).FirstOrDefault();

            if (orderToUpdate == null)
            {
                return NotFound();
            }

            if (orderToUpdate.OrderStatus != OrderStatus.Waiting)
            {
                return BadRequest("Order can be changed by user only in waiting state!");
            }

            orderToUpdate = Mapper.Map<Order>(model);

            this.Data.Orders.Update(orderToUpdate);
            this.Data.Orders.SaveChanges();

            var orderVM = Mapper.Map<OrderDetailsVM>(orderToUpdate);
            this.bridge.UpdateOrder(orderVM, orderToUpdate.District.DistrictId);

            return Ok(model);

        }

        /// <summary>
        /// Cancels a client order. Possible only if it's in the "Waiting" state.
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

            if (orderToCancel.OrderStatus != OrderStatus.Waiting)
            {
                return BadRequest("Order can be changed only in waiting state!");
            }

            orderToCancel.OrderStatus = OrderStatus.Cancelled;
            var orderToCancelModel = Mapper.Map<OrderDetailsVM>(orderToCancel);
            this.Data.Orders.Update(orderToCancel);
            this.Data.SaveChanges();

            this.bridge.CancelOrder(orderToCancel.OrderId, orderToCancel.District.DistrictId);

            return Ok(orderToCancel);
        }
    }
}
