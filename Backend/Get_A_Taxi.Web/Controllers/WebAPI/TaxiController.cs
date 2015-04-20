using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using Get_A_Taxi.Data;
using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure;
using Get_A_Taxi.Web.Infrastructure.Bridges;
using Get_A_Taxi.Web.Models;

namespace Get_A_Taxi.Web.Controllers.WebAPI
{
    /// <summary>
    /// Manages driver assignment to a taxi
    /// </summary>
    [AuthorizeRoles(UserRole = UserRoles.Driver)]
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
        /// <returns>List of all free for assignment taxi data models</returns>
        [HttpGet]
        public IHttpActionResult Get()
        {
            var driver = this.GetUser();
            var district = driver.District;
            var freeTaxies = this.Data.Taxies.All()
                .Where(t => t.Driver.Id == null && t.Status == TaxiStatus.OffDuty)
                .AsQueryable()
                .Take(RESULTS_COUNT)
                .Project().To<TaxiDTO>().ToList();
            return Ok(freeTaxies);
        }

        /// <summary>
        /// Get taxi details, only if the driver and the taxi are in the same district
        /// </summary>
        /// <param name="id">Id of the taxi</param>
        /// <returns>Taxi details data model</returns>
        public IHttpActionResult Get(int id)
        {
            var driver = this.GetUser();
            var districtId = driver.District.DistrictId;
            var taxiDetails = this.Data.Taxies
                .SearchFor(t => t.TaxiId == id && t.District.DistrictId == districtId)
                .Project().To<TaxiDetailsDTO>()
                .FirstOrDefault();


            if (taxiDetails == null)
            {
                return NotFound();
            }

            if (taxiDetails.DistrictId != districtId)
            {
                return BadRequest("Taxi is not in your district!");
            }

            return Ok(taxiDetails);
        }

        /// <summary>
        /// Gets a page of all available for driver assignment taxies for the driver's district
        /// </summary>
        /// <param name="page">The page number</param>
        /// <returns>List of free for assignment taxies' data models</returns>
        public IHttpActionResult GetPaged(int page)
        {
            var driver = this.GetUser();
            var district = driver.District;
            var freeTaxies = this.Data.Taxies.All()
                .Where(t => t.Driver.Id == null && t.Status == TaxiStatus.OffDuty)
                .AsQueryable()
                .Skip(page * RESULTS_COUNT)
                .Take(RESULTS_COUNT)
                .Project().To<TaxiDTO>().ToList();
            return Ok(freeTaxies);
        }

        /// <summary>
        /// Assign a driver to a taxi and make it available
        /// </summary>
        /// <param name="model">The data model of the taxi the driver will be assigned to</param>
        /// <returns>The detailed data model of the taxi that the driver was assigned to</returns>
        public IHttpActionResult Post([FromBody]TaxiDTO model)
        {
            var driver = this.GetUser();
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

            if (taxi.Status != TaxiStatus.OffDuty)
            {
                return BadRequest("Taxi is not off-duty!");
            }

            var driversCurrentTaxi = this.Data.Taxies.SearchFor(t => t.Driver.Id == driver.Id).FirstOrDefault();
            if (driversCurrentTaxi != null)
            {
                return BadRequest("You are already assigned to another taxi: " + driversCurrentTaxi.Plate + "!");
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
        /// Change taxi status
        /// </summary>
        /// <param name="model">The taxi's new data model</param>
        /// <returns>Appropriate responce</returns>
        public IHttpActionResult Put([FromBody]TaxiDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model state!");
            }
            var driver = this.GetUser();
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

            if (taxi.Status == TaxiStatus.Busy && model.OnDuty == false)
            {
                return BadRequest("Taxi is serving an order - cannot get off-duty!");
            }



            taxi.Latitude = model.Latitude;
            taxi.Longitude = model.Longitude;
            taxi.Status = FromModelStatus(model);

            this.Data.Taxies.Update(taxi);
            this.Data.Taxies.SaveChanges();

            // Update the district about the new taxi state
            this.taxiesBridge.TaxiUpdated(model, districtId);
            return Ok();
        }


        /// <summary>
        /// Unassigns a driver from a taxi
        /// </summary>
        /// <param name="taxiId">The id of a taxi the driver will be assigned to</param>
        /// <returns>The taxi's data model that the driver was assigned to</returns>
        public IHttpActionResult Delete(int taxiId)
        {
            var driver = this.GetUser();
            var districtId = driver.District.DistrictId;
            var taxi = this.Data.Taxies
                .SearchFor(t => t.TaxiId == taxiId && t.Driver.Id == driver.Id)
                .FirstOrDefault();

            if (taxi == null)
            {
                return NotFound();
            }

            if (taxi.Status != TaxiStatus.Available)
            {
                return BadRequest("Taxi must be on-duty to be un-assigned!");
            }

            taxi.Driver = null;
            taxi.Status = TaxiStatus.OffDuty;

            this.Data.Taxies.Update(taxi);
            this.Data.Taxies.SaveChanges();

            // Notify district about taxi getting off-duty
            this.taxiesBridge.TaxiOffDuty(taxi.TaxiId, districtId);
            return Ok();
        }

        #region Helpers
        [NonAction]
        private TaxiStatus FromModelStatus(TaxiDTO model)
        {
            if (model.IsAvailable)
            {
                if (!model.OnDuty)
                {
                    return TaxiStatus.OffDuty;
                }
                else
                {
                    return TaxiStatus.Available;
                }
            }
            else
            {
                return TaxiStatus.Busy;
            }
        }
        #endregion
    }
}
