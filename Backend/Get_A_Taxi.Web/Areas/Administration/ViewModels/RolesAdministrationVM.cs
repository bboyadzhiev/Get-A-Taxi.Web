using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using Get_A_Taxi.Web.ViewModels;

namespace Get_A_Taxi.Web.Areas.Administration.ViewModels
{
    public class RolesAdministrationVM
    {
       // [Key]
      //  public int Id { get; set; }
        public ICollection<UserItemViewModel> Accounts { get; set; }

       
       // public string SelectedRoleId { get; set; }
        public List<SelectListItem> UserRoles { get; set; }
    }
}