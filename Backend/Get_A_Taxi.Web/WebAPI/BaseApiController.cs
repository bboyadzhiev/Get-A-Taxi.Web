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

        //private ApplicationUser _member;

        
        ////try  n
        //private ApplicationUserManager _userManager;

        //public ApplicationUserManager UserManager1
        //{
        //    get
        //    {
        //        return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //    }
        //    private set
        //    {
        //        _userManager = value;
        //    }
        //}

        //// try 2
        //public ApplicationUserManager UserManager2
        //{
        //    get { return HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
        //}
        //public string UserIdentityId
        //{
        //    get
        //    {
        //        //var user = UserManager.FindByName(User.Identity.Name);
        //        //return user.Id;
        //        var username = RequestContext.Principal.Identity.GetUserName();
                
        //       // var userobject = UserManager.FindByNameAsync(uident.Name);
        //     //   ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
        //     //   string username = identity.Claims.First().Value;
        //        var userobject = UserManager2.FindByNameAsync(username);
        //        var userid = userobject.Result.Id;
        //        return userid;
        //    }
        //}

        //public ApplicationUser UserRecord
        //{
        //    get
        //    {
        //        if (_member != null)
        //        {
        //            return _member;
        //        }
        //        _member = UserManager2.FindByEmail(Thread.CurrentPrincipal.Identity.Name);
        //        return _member;
        //    }
        //    set { _member = value; }
        //}


        #endregion
    }
}
