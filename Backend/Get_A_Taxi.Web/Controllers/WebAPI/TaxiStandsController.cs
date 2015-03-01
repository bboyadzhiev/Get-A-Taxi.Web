using Get_A_Taxi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Get_A_Taxi.Web.Controllers.WebAPI
{
    [RoutePrefix("api/TaxiStands")]
    public class TaxiStandsController : BaseApiController
    {

        public TaxiStandsController(IGetATaxiData data)
            :base(data)
        {

        }
        public IHttpActionResult Get(int districtId)
        {
            if (districtId != null)
            {
                var taxiStands = this.data.Stands.All().Where(s => s.District.DistrictId == districtId).ToList();
                return Ok(taxiStands);
            }
            return NotFound();
        }
    }
}
