namespace Get_A_Taxi.Web.WebAPI
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Linq;

    using Get_A_Taxi.Data;
    using Get_A_Taxi.Models;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;

    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.OAuth;

    /// <summary>
    /// Base WebAPI Controller
    /// </summary>
    public class BaseApiController : ApiController
    {
        protected IGetATaxiData Data;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data">Instance of the data context</param>
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
