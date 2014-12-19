using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Get_A_Taxi.Data;

namespace Get_A_Taxi.WebAPI.Controllers
{
    public class BaseController : ApiController  
    {
        protected IGetATaxiData data;
        public BaseController(IGetATaxiData data)
        {
            this.data = data;
        }
    }
}