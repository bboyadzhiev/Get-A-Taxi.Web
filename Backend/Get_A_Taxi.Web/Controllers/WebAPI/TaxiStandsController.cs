using Get_A_Taxi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper.QueryableExtensions;
using Get_A_Taxi.Web.Models;

namespace Get_A_Taxi.Web.Controllers.WebAPI
{
    [Authorize]
    [RoutePrefix("api/TaxiStands")]
    public class TaxiStandsController : BaseApiController
    {

        public TaxiStandsController(IGetATaxiData data)
            : base(data)
        {
        }

        /// <summary>
        /// Get all Taxi Stands for a district
        /// </summary>
        /// <param name="districtId">The District's ID</param>
        /// <returns>A list of taxi stands</returns>
        [HttpGet]
        public IHttpActionResult Get(int districtId)
        {
            var taxiStands = this.Data.Stands.All()
                .Where(s => s.District.DistrictId == districtId)
                .Project().To<TaxiStandDTO>()
                .ToList();

            return Ok(taxiStands);
        }
    }
}
