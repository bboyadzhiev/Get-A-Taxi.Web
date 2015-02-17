namespace Get_A_Taxi.Web.Controllers
{
    using Get_A_Taxi.Data;
    using System.Web.Http;
    public class BaseApiController : ApiController
    {
        protected IGetATaxiData data;
        public BaseApiController(IGetATaxiData data)
        {
            this.data = data;
        }
    }
}
