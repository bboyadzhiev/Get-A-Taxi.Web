namespace Get_A_Taxi.Web.Controllers
{
    using Get_A_Taxi.Data;
    using Get_A_Taxi.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using System.Web.Http;
    using System.Linq;
    using System.Web;
    using System.Threading;
    using System;
    using System.Web.Routing;
    using System.Security.Claims;

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

        private ApplicationUser _member;

        public ApplicationUserManager UserManager
        {
            get { return HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
        }

        public string UserIdentityId
        {
            get
            {
                //var user = UserManager.FindByName(User.Identity.Name);
                //return user.Id;

               // var userobject = UserManager.FindByNameAsync(uident.Name);
                ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
                string username = identity.Claims.First().Value;
                var userobject = UserManager.FindByNameAsync(username);
                var userid = userobject.Result.Id;
                return userid;
            }
        }

        public ApplicationUser UserRecord
        {
            get
            {
                if (_member != null)
                {
                    return _member;
                }
                _member = UserManager.FindByEmail(Thread.CurrentPrincipal.Identity.Name);
                return _member;
            }
            set { _member = value; }
        }


        #endregion
    }
}
