namespace Get_A_Taxi.Web.Controllers
{
    using Get_A_Taxi.Data;
    using Get_A_Taxi.Models;
    using Microsoft.AspNet.Identity;
    using System.Web.Http;
    using System.Linq;

    public class BaseApiController : ApiController
    {
        protected IGetATaxiData Data;
        public BaseApiController(IGetATaxiData data)
        {
            this.Data = data;
        }
        
        #region Helpers
        [NonAction]
        public ApplicationUser GetUser()
        {
            string userId = this.User.Identity.GetUserId();
            var user = this.Data.Users.All().FirstOrDefault(u => u.Id == userId);
            return user;
        }
        #endregion
    }
}
