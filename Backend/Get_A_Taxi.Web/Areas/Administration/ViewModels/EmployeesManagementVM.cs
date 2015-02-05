using Get_A_Taxi.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Get_A_Taxi.Web.Areas.Management.ViewModels
{
    public class EmployeesManagementVM
    {
        public ICollection<UserItemViewModel> Accounts { get; set; }

    }
}