using Get_A_Taxi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper.QueryableExtensions;
using Get_A_Taxi.Web.Models;
using Get_A_Taxi.Web.Infrastructure.Authorization;
using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.Services.Contracts;
using Get_A_Taxi.Web.Infrastructure.Services;

namespace Get_A_Taxi.Web.WebAPI
{
    [Authorize]
    [RoutePrefix("api/TaxiStands")]
    public class TaxiStandsController : BaseApiController, IRESTController<TaxiStandDTO>
    {
        private ITaxiStandService taxiStandService;
        private const int TAXI_STANDS_RESULTS_DEFAULT_COUNT = 10;

        public TaxiStandsController(IGetATaxiData data)
            : base(data)
        {
            this.taxiStandService = new TaxiStandService(data);
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get()
        {
            return BadRequest();
        }

        /// <summary>
        /// Get taxi stand by id
        /// </summary>
        /// <param name="id">The id of the taxi stand</param>
        /// <returns>The taxi stand's data model</returns>
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var taxiStand = this.taxiStandService.GetAll().Where(t => t.TaxiStandId == id).Project().To<TaxiStandDTO>();
            return Ok(taxiStand);
        }

        /// <summary>
        /// Get all Taxi Stands for the nearest district by given coordinates
        /// </summary>
        /// <param name="lat">The latitude</param>
        /// <param name="lon">The longitude</param>
        /// <returns>List of top 10 nearest locations</returns>
        [HttpGet]
        public IHttpActionResult Get(double lat, double lon)
        {
            var taxiStands = this.taxiStandService.GetAll();
            taxiStands = this.taxiStandService.ActiveOnly(taxiStands, true);
            var taxiStandsDTOList = this.taxiStandService.ByNearestLocation(taxiStands, lat, lon)
                .Take(TAXI_STANDS_RESULTS_DEFAULT_COUNT)
                .Project().To<TaxiStandDTO>()
                .ToList();

            return Ok(taxiStandsDTOList);
        }

        /// <summary>
        /// Used by taxi driver to list a page of all taxi stands in his district
        /// </summary>
        /// <param name="page">Page number</param>
        /// <returns>List of taxi stands</returns>
        [AuthorizeRoles(UserRole = UserRoles.Driver)]
        [HttpGet]
        public IHttpActionResult GetPaged(int page)
        {
            var driver = this.GetUser();
            var taxiStands = this.taxiStandService.GetAll();
            taxiStands = this.taxiStandService.ActiveOnly(taxiStands, true);
            var taxiStandsDTOList = this.taxiStandService.ByDistrict(taxiStands, driver.District.DistrictId)
                .Skip(page * TAXI_STANDS_RESULTS_DEFAULT_COUNT)
                .Take(TAXI_STANDS_RESULTS_DEFAULT_COUNT)
                .Project().To<TaxiStandDTO>()
                .ToList();
            return Ok(taxiStandsDTOList);
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Post(TaxiStandDTO model)
        {
            return BadRequest();
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult Put(TaxiStandDTO model)
        {
            return BadRequest();
        }
     
        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            return BadRequest();
        }

    }
}
