using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using Get_A_Taxi.Data;
using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure;
using Get_A_Taxi.Web.Infrastructure.Bridges;
using Get_A_Taxi.Web.Models;
using System.Security.Principal;

namespace Get_A_Taxi.Web.WebAPI
{
    /// <summary>
    /// Manages driver assignment to a taxi
    /// </summary>
    [AuthorizeWebApi(UserRole = UserRoles.Driver)]
    [RoutePrefix("api/Taxi")]
    public class TaxiController : BaseApiController, IRESTController<TaxiDTO>
    {

        private const int RESULTS_COUNT = 10;
        private ITaxiesBridge taxiesBridge;

        public TaxiController(IGetATaxiData data, ITaxiesBridge taxiesBridge)
            : base(data)
        {
            this.taxiesBridge = taxiesBridge;
        }

        /// <summary>
        /// Gets all available for driver assignment taxies for the driver's district
        /// </summary>
        /// <returns>List of all free for assignment taxies' data models</returns>
        [HttpGet]
        public IHttpActionResult Get()
        {
            var driver = this.GetDriver();
            
            // check if already assigned to a taxi
            var taxiWithThisDriver = this.Data.Taxies.SearchFor(t => t.Driver.Id == driver.Id).FirstOrDefault();
            if (taxiWithThisDriver != null)
            {
                var assignedTaxi = Mapper.Map<TaxiDetailsDTO>(taxiWithThisDriver);
                return Ok(new List<TaxiDetailsDTO>() { assignedTaxi });
            }

            var districtId = driver.District.DistrictId;
            var freeTaxies = this.Data.Taxies.All()
                .Where(t => t.District.DistrictId == districtId && t.Status == TaxiStatus.Unassigned && t.Driver == null)
                .AsQueryable()
                .Take(RESULTS_COUNT)
                .Project().To<TaxiDetailsDTO>().ToList();
            return Ok(freeTaxies);
        }

        /// <summary>
        /// Get taxi details
        /// </summary>
        /// <param name="id">Id of the taxi</param>
        /// <returns>Taxi details data model</returns>
        [AllowAnonymous]
        public IHttpActionResult Get(int id)
        {
            var taxi = this.Data.Taxies
                .SearchFor(t => t.TaxiId == id)
                .FirstOrDefault();

            if (taxi == null)
            {
                return NotFound();
            }

            if (taxi.Status == TaxiStatus.Decommissioned)
            {
                return BadRequest("Taxi has been decommisioned!");
            }

            var taxiDetails = Mapper.Map<OrderDetailsDTO>(taxi);

            return Ok(taxiDetails);
        }

        /// <summary>
        /// Gets a page of all available for driver assignment taxies for the driver's district
        /// </summary>
        /// <param name="page">The page number</param>
        /// <returns>List of free for assignment taxies' data models</returns>
        public IHttpActionResult GetPaged(int page)
        {
            var driver = this.GetDriver();
            var district = driver.District;
            var freeTaxies = this.Data.Taxies.All()
                .Where(t => t.Driver.Id == null && t.Status == TaxiStatus.Unassigned)
                .AsQueryable()
                .Skip(page * RESULTS_COUNT)
                .Take(RESULTS_COUNT)
                .Project().To<TaxiDetailsDTO>().ToList();
            return Ok(freeTaxies);
        }

        /// <summary>
        /// Assign a driver to a taxi and make it available
        /// </summary>
        /// <param name="model">The data model of the taxi the driver will be assigned to</param>
        /// <returns>The detailed data model of the taxi that the driver was assigned to</returns>
        public IHttpActionResult Post([FromBody]TaxiDTO model)
        {
            var driver = this.GetDriver();
            var districtId = driver.District.DistrictId;
            var taxi = this.Data.Taxies
                .SearchFor(t => t.TaxiId == model.TaxiId)
                .FirstOrDefault();
            if (taxi == null)
            {
                return NotFound();
            }

            if (taxi.District.DistrictId != districtId)
            {
                return BadRequest("Taxi is not in your district!");
            }

            var taxiWithThisDriver = this.Data.Taxies.SearchFor(t => t.Driver.Id == driver.Id).FirstOrDefault();
            if (taxiWithThisDriver != null )
            {
                if (taxiWithThisDriver.TaxiId == taxi.TaxiId)
                {
                    var assignedTaxi = Mapper.Map<TaxiDetailsDTO>(taxi);
                    return Ok(assignedTaxi);
                }

                if (taxiWithThisDriver.TaxiId != taxi.TaxiId)
                {
                 return BadRequest("You are already assigned to another taxi: " + taxiWithThisDriver.Plate + "!");
                }
            }

            if (taxi.Status != TaxiStatus.Unassigned)
            {
                return BadRequest("Taxi is not available for driver change!");
            }


            taxi.Driver = driver;
            taxi.Latitude = model.Latitude;
            taxi.Longitude = model.Longitude;
            taxi.Status = TaxiStatus.Available;

            this.Data.Taxies.Update(taxi);
            this.Data.Taxies.SaveChanges();

            // Notify the district about the taxi assignment
            var taxiDetails = Mapper.Map<TaxiDetailsDTO>(taxi);
            this.taxiesBridge.TaxiOnDuty(taxiDetails, districtId);

            return Ok(taxiDetails);
        }

        /// <summary>
        /// Update taxi's details and status
        /// </summary>
        /// <param name="model">The taxi's new data model</param>
        /// <returns>The updated taxi's Taxi Status id (Available, Busy, OffDuty, etc.)</returns>
        public IHttpActionResult Put([FromBody]TaxiDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model state!");
            }
            var driver = this.GetDriver();
            var districtId = driver.District.DistrictId;
            var taxi = this.Data.Taxies
                .SearchFor(t => t.TaxiId == model.TaxiId && t.Driver.Id == driver.Id)
                .FirstOrDefault();
            if (taxi == null)
            {
                return NotFound();
            }

            if (taxi.District.DistrictId != districtId)
            {
                return BadRequest("Taxi is not in your district!");
            }

            if (taxi.Status == TaxiStatus.Decommissioned)
            {
                return BadRequest("Unauthorized operation!");
            }

            if (taxi.Status == TaxiStatus.Busy && model.Status == (int)TaxiStatus.OffDuty)
            {
                return BadRequest("Taxi is serving an order - cannot get off-duty!");
            }

            taxi.Latitude = model.Latitude;
            taxi.Longitude = model.Longitude;
            taxi.Status = (TaxiStatus)model.Status;

            this.Data.Taxies.Update(taxi);
            this.Data.Taxies.SaveChanges();

            if (taxi.Status == TaxiStatus.OffDuty)
            {
                this.taxiesBridge.TaxiOffDuty(taxi.TaxiId, districtId);
            } else if (taxi.Status == TaxiStatus.Available)
            {
                var onDutyModel = Mapper.Map<TaxiDetailsDTO>(taxi);
                this.taxiesBridge.TaxiOnDuty(onDutyModel, districtId);
            }
            else
            {
                // Update the district about the new taxi state
                this.taxiesBridge.TaxiUpdated(model, districtId);
            }

            return Ok((int)taxi.Status);
        }

        /// <summary>
        /// Unassigns a driver from a taxi
        /// </summary>
        /// <param name="id">The id of a taxi the driver will be assigned to</param>
        /// <returns>The taxi's data model that the driver was assigned to</returns>
        public IHttpActionResult Delete(int id)
        {
            var driver = this.GetDriver();
            var districtId = driver.District.DistrictId;
            var taxi = this.Data.Taxies
                .SearchFor(t => t.TaxiId == id && t.Driver.Id == driver.Id)
                .FirstOrDefault();

            if (taxi == null)
            {
                return NotFound();
            }

            //var order = this.Data.Orders.All().Where(o => o.AssignedTaxi.TaxiId == taxi.TaxiId && o.OrderStatus == OrderStatus.Waiting || o.OrderStatus == OrderStatus.InProgress).FirstOrDefault();


            if (taxi.Status != TaxiStatus.Available)
            {
                return BadRequest("Taxi must be on-duty to be un-assigned!");
            }

            taxi.Driver = null;
            taxi.Status = TaxiStatus.Unassigned;

            this.Data.Taxies.Update(taxi);
            this.Data.Taxies.SaveChanges();

            // Notify district about taxi getting off-duty
            this.taxiesBridge.TaxiOffDuty(taxi.TaxiId, districtId);
            return Ok();
        }

        #region Helpers

        private ApplicationUser GetDriver()
        {
            Guid userGuid = (Guid)ActionContext.Request.Properties["userId"];
            var userId = userGuid.ToString();

            var driver = this.Data.Users.SearchFor(u => u.Id == userId).FirstOrDefault();
            return driver;
        }
        #endregion
    }
}
