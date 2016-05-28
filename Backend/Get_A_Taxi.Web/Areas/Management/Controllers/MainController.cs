using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.Authorization;
using Get_A_Taxi.Web.Infrastructure.LocalResource;
using System.Web.Mvc;

namespace Get_A_Taxi.Web.Areas.Management.Controllers
{
    [AuthorizeRoles(UserRole = UserRoles.Administrator, SecondRole = UserRoles.Manager)]
    public class MainController : LocalizationController
    {
        // GET: Management/Main
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Employees()
        {
            return View("ManageEmployees");
        }
    }
}