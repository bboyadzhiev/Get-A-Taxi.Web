﻿using Get_A_Taxi.Data;
using Get_A_Taxi.Web.Infrastructure.Bridges;
using Get_A_Taxi.Web.Models;
using Get_A_Taxi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper.QueryableExtensions;
using Get_A_Taxi.Web.Infrastructure;
using AutoMapper;
using Get_A_Taxi.Web.ViewModels;

namespace Get_A_Taxi.Web.Controllers.WebAPI
{
    /// <summary>
    /// Manages the orders for taxies
    /// </summary>
    [AuthorizeRoles(UserRole = UserRoles.Driver)]
    [RoutePrefix("api/TaxiOrders")]
    public class TaxiOrdersController : BaseApiController, IRESTController<OrderDetailsDTO>
    {

        private const int RESULTS_COUNT = 10;
        private IOrderBridge ordersBridge;
        private ITaxiesBridge taxiesBrigde;
        public TaxiOrdersController(IGetATaxiData data, IOrderBridge ordersBridge, ITaxiesBridge taxiesBridge)
            : base(data)
        {
            this.ordersBridge = ordersBridge;
            this.taxiesBrigde = taxiesBrigde;
        }

        /// <summary>
        /// Get all waiting orders for the district, order ascending by distance to taxi's position
        /// Returns top RESULTS_COUNT results
        /// </summary>
        /// <returns>A list of order data models</returns>
        public IHttpActionResult Get()
        {
            var driver = this.GetUser();
            var taxi = this.Data.Taxies.SearchFor(t => t.Driver.Id == driver.Id).FirstOrDefault();
            var check = CheckTaxi(taxi);
            if (check != null)
            {
                return check;
            }

            var districtId = driver.District.DistrictId;
            var orders = this.Data.Orders.All()
                .Where(o => o.District.DistrictId == districtId && o.OrderStatus == OrderStatus.Waiting)
                .OrderBy(o => (o.OrderLatitude - taxi.Latitude) + (o.OrderLongitude - taxi.Longitude))
                .Take(RESULTS_COUNT)
                .Project().To<OrderDTO>()
                .ToList();

            return Ok(orders);

        }

        /// <summary>
        /// Get the detais of a specific order
        /// </summary>
        /// <param name="id">The order's id</param>
        /// <returns>An order details data model</returns>
        public IHttpActionResult Get(int id)
        {
            var driver = this.GetUser();
            var taxi = this.Data.Taxies.SearchFor(t => t.Driver.Id == driver.Id).FirstOrDefault();
            var check = CheckTaxi(taxi);
            if (check != null)
            {
                return check;
            }

            var orderDetails = this.Data.Orders.SearchFor(o => o.OrderId == id).Project().To<OrderDetailsDTO>().FirstOrDefault();

            return Ok(orderDetails);
        }

        /// <summary>
        /// Get a specific page of all the waiting orders for the district, 
        /// ordered ascending by distance to taxi's position
        /// </summary>
        /// <param name="page">The page number</param>
        /// <returns>A list of order data models</returns>
        public IHttpActionResult GetPaged(int page)
        {
            var driver = this.GetUser();
            var taxi = this.Data.Taxies.SearchFor(t => t.Driver.Id == driver.Id).FirstOrDefault();
            var check = CheckTaxi(taxi);
            if (check != null)
            {
                return check;
            }

            var districtId = driver.District.DistrictId;
            var orders = this.Data.Orders.All()
                .Where(o => o.District.DistrictId == districtId)
                .OrderBy(o => (o.OrderLatitude - taxi.Latitude) + (o.OrderLongitude - taxi.Longitude))
                .Skip(page * RESULTS_COUNT)
                .Take(RESULTS_COUNT)
                .Project().To<OrderDTO>()
                .ToList();

            return Ok(orders);
        }

        // TODO: Review the purpose of this action
        /// <summary>
        /// Place new order from the taxi driver 
        /// (customer got the taxi on a taxi stand or by hailing it). 
        /// Customer's Id will be the Driver's Id
        /// </summary>
        /// <param name="model">A detailed order's data model</param>
        /// <returns>The new order's data model</returns>
        [HttpPost]
        public IHttpActionResult Post([FromBody]OrderDetailsDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model state!");
            }

            var driver = this.GetUser();
            var taxi = this.Data.Taxies.SearchFor(t => t.Driver.Id == driver.Id).FirstOrDefault();
            var taxiErrorCheck = CheckTaxi(taxi);
            if (taxiErrorCheck != null)
            {
                return taxiErrorCheck;
            }

            var newOrder = Mapper.Map<Order>(model);

            newOrder.District = driver.District;
            newOrder.Driver = driver;
            newOrder.Customer = driver;
            newOrder.OrderStatus = OrderStatus.InProgress;
            newOrder.AssignedTaxi = taxi;

            this.Data.Orders.Add(newOrder);
            //this.Data.Orders.SaveChanges();

            // The taxi's locatin is at the new order location
            taxi.Latitude = model.OrderLatitude;
            taxi.Longitude = model.OrderLongitude;
            taxi.Status = TaxiStatus.Busy;

            this.Data.Taxies.Update(taxi);
            this.Data.SaveChanges();

            // Prepare the new order for the Taxi Driver
            var addedOrder = this.Data.Orders.SearchFor(o => o.OrderId == newOrder.OrderId).FirstOrDefault();
            var addedOrderModel = Mapper.Map<OrderDetailsDTO>(addedOrder);

            // Notify the district about the new order
            this.ordersBridge.AddOrder(addedOrderModel, addedOrder.District.DistrictId);

            // When an order is taken the taxi is automatically 
            // Notify the district about the new taxi state
            var taxiDM = Mapper.Map<TaxiDTO>(taxi);
            this.taxiesBrigde.TaxiUpdated(taxiDM, taxi.District.DistrictId);

            return Ok(addedOrderModel);

        }

        /// <summary>
        /// Assign order to taxi
        /// </summary>
        /// <param name="orderId">The order's id</param>
        /// <param name="driverId">The taxi driver's id</param>
        /// <returns>The assigned order data model</returns>
        [HttpPut]
        public IHttpActionResult Put(int orderId)
        {
            var orderToAssign = this.Data.Orders.SearchFor(o => o.OrderId == orderId).FirstOrDefault();
            if (orderToAssign == null)
            {
                return NotFound();
            }

            var driver = this.GetUser();
            var taxi = this.Data.Taxies.SearchFor(t => t.Driver.Id == driver.Id).FirstOrDefault();
            var taxiErrorCheck = CheckTaxi(taxi);
            if (taxiErrorCheck != null)
            {
                return taxiErrorCheck;
            }

            // Check if order is still waiting for an assignment
            //return BadRequest("The order is already processed!");


            if (orderToAssign.OrderStatus == OrderStatus.Waiting)
            {
                // Checks passed at this point, assigning order
                orderToAssign.AssignedTaxi = taxi;
                orderToAssign.Driver = driver;
                orderToAssign.OrderStatus = OrderStatus.InProgress;
                this.Data.Orders.Update(orderToAssign);
                this.Data.Orders.SaveChanges();

                // Notify all about order's assignment
                this.ordersBridge.AssignOrder(orderToAssign.OrderId, taxi.TaxiId, orderToAssign.District.DistrictId);
            }
            else if (orderToAssign.OrderStatus == OrderStatus.InProgress
                && orderToAssign.AssignedTaxi.TaxiId == taxi.TaxiId)
            {
                orderToAssign.AssignedTaxi = null;
                orderToAssign.Driver = null;
                orderToAssign.OrderStatus = OrderStatus.Waiting;
                this.Data.Orders.Update(orderToAssign);
                this.Data.Orders.SaveChanges();

                this.ordersBridge.CancelOrder(orderToAssign.OrderId, driver.District.DistrictId);
            }

            var assignedOrderModel = Mapper.Map<OrderDetailsDTO>(orderToAssign);
            return Ok(assignedOrderModel);
        }

        // TODO: Review order changes by taxi
        /// <summary>
        /// Update order's details by taxi driver
        /// </summary>
        /// <param name="model">A detailed order's data model</param>
        /// <returns>The updated order's data model</returns>
        [HttpPut]
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

            var driver = this.GetUser();
            var taxi = this.Data.Taxies.SearchFor(t => t.Driver.Id == driver.Id).FirstOrDefault();
            var taxiErrorCheck = CheckTaxi(taxi);
            if (taxiErrorCheck != null)
            {
                return taxiErrorCheck;
            }

            if (orderToUpdate.OrderStatus != OrderStatus.Waiting && orderToUpdate.OrderStatus != OrderStatus.InProgress)
            {
                return BadRequest("Order can be changed by taxi driver only in waiting or progress state!");
            }

            if (orderToUpdate.OrderStatus == OrderStatus.InProgress)
            {
                if (orderToUpdate.AssignedTaxi.TaxiId != taxi.TaxiId)
                {
                    return BadRequest("This order is already assigned to another taxi!");
                }
            }

            // Checks passed at this point, updating order's details
            orderToUpdate = Mapper.Map<Order>(model);

            if (model.IsWaiting == true && model.IsFinished == true)
            {
                orderToUpdate.OrderStatus = OrderStatus.Cancelled;
            }
            if (model.IsWaiting == false && model.IsFinished == true)
            {
                orderToUpdate.OrderStatus = OrderStatus.Finished;
            }
            if (model.IsWaiting == false && model.IsFinished == false)
            {
                orderToUpdate.OrderStatus = OrderStatus.InProgress;
            }

            this.Data.Orders.Update(orderToUpdate);
            this.Data.Orders.SaveChanges();

            // Notify all about order's update
            this.ordersBridge.UpdateOrder(model, orderToUpdate.District.DistrictId);

            return Ok(model);
        }




        /// <summary>
        /// Cancel order by taxi driver
        /// </summary>
        /// <param name="id">The order to be cancelled </param>
        /// <returns>The cancelled order id</returns>
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var driver = this.GetUser();
            var taxi = this.Data.Taxies.SearchFor(t => t.Driver.Id == driver.Id).FirstOrDefault();
            var orderToCancel = this.Data.Orders.SearchFor(o => o.OrderId == id && o.AssignedTaxi.TaxiId == taxi.TaxiId).FirstOrDefault();
            if (orderToCancel == null)
            {
                return NotFound();
            }

            if (orderToCancel.OrderStatus != OrderStatus.InProgress)
            {
                return BadRequest("Order not in progress - cannot be cancelled!");
            }

            //Checks passed
            orderToCancel.OrderStatus = OrderStatus.Cancelled;
            this.Data.Orders.Update(orderToCancel);
            this.Data.Orders.SaveChanges();

            this.ordersBridge.CancelOrder(orderToCancel.OrderId, orderToCancel.District.DistrictId);

            return Ok(id);
        }

        #region Helpers
        [NonAction]
        private IHttpActionResult CheckTaxi(Taxi taxi)
        {

            if (taxi == null)
            {
                return BadRequest("No taxi assigned to this driver!");
            }
            if (taxi.Status == TaxiStatus.Decommissioned)
            {
                return BadRequest("Taxi " + taxi.Plate + " is decommissioned! It cannot serve any order!");
            }
            return null;
        }
        #endregion
    }
}
