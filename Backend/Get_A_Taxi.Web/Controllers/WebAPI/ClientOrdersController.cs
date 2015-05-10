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
        /// Get assigned client order by id
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

            //if (order.AssignedTaxi == null)
            //{
            //    return BadRequest("Order not yet assigned");
            //}

            var orderDTO = Mapper.Map<Order, AssignedOrderDTO>(order);

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
        /// <returns>The data model of the new order</returns>
        [HttpPost]
        public IHttpActionResult Post([FromBody]OrderDetailsDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model state!");
            }

            var user = GetUser();

            // Checking for unfinished orders
            var order = this.Data.Orders.All()
                .Where(o => o.Customer.Id == user.Id && (o.OrderStatus == OrderStatus.Waiting || o.OrderStatus == OrderStatus.InProgress))
                .FirstOrDefault();

            if (order != null)
            {
                // Previous order is not finished, updating with new details and returning new model
                order.OrderLatitude = model.OrderLatitude;
                order.OrderLongitude = model.OrderLongitude;
                order.OrderAddress = model.OrderAddress;
                order.UserComment = model.UserComment;
                order.DestinationAddress = model.DestinationAddress;
                order.DestinationLatitude = model.DestinationLatitude;
                order.DestinationLongitude = model.DestinationLongitude;

                this.Data.Orders.Update(order);
                this.Data.Orders.SaveChanges();

                var updatedOrder = this.Data.Orders.SearchFor(o => o.OrderId == order.OrderId).FirstOrDefault();
                var updatedOrderModel = Mapper.Map<OrderDTO>(updatedOrder);

                //IHttpActionResult response;
                //HttpResponseMessage responseMsg = Request.CreateResponse<OrderDTO>(HttpStatusCode.Found, updatedOrderModel);
                //response = ResponseMessage(responseMsg);
                //return response;
                return Ok(updatedOrderModel);
            }

            // HACK: Review distance calculation
            // New order
            // Finds the closes district
            var closestDistrict = this.Data.Districts.All().OrderBy(d => ((d.CenterLatitude - model.OrderLatitude) + (d.CenterLongitude - model.OrderLongitude))).FirstOrDefault();
            order = Mapper.Map<Order>(model);

            order.District = closestDistrict;
            order.Customer = user;
            order.OrderStatus = OrderStatus.Waiting;

            this.Data.Orders.Add(order);
            this.Data.Orders.SaveChanges();

            var addedOrder = this.Data.Orders.SearchFor(o => o.OrderId == order.OrderId).FirstOrDefault();
            var addedOrderModel = Mapper.Map<OrderDTO>(addedOrder);

            var orderDTO = Mapper.Map<OrderDetailsDTO>(addedOrder);
            this.bridge.AddOrder(orderDTO, addedOrder.District.DistrictId);

            return Ok(addedOrderModel);
        }

        /// <summary>
        /// Updates a client order. Possible only if it's in the "Waiting" state.
        /// </summary>
        /// <param name="model">The updated order's data model</param>
        /// <returns>The updated order's data model</returns>
        public IHttpActionResult Put([FromBody]OrderDetailsDTO model)
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

            var orderDTO = Mapper.Map<OrderDetailsDTO>(orderToUpdate);
            this.bridge.UpdateOrder(orderDTO, orderToUpdate.District.DistrictId);

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

            if (orderToCancel.OrderStatus == OrderStatus.Waiting || orderToCancel.OrderStatus == OrderStatus.InProgress)
            {
                orderToCancel.OrderStatus = OrderStatus.Cancelled;

                this.Data.Orders.Update(orderToCancel);
                this.Data.SaveChanges();

                this.bridge.CancelOrder(orderToCancel.OrderId, orderToCancel.District.DistrictId);

                var orderDM = Mapper.Map<Order, OrderDTO>(orderToCancel);

                return Ok(orderDM);
            }

            return BadRequest("Order can be cancelled only in waiting or progress states!");
        }
    }
}
