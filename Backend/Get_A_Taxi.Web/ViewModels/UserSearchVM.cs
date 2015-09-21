using Get_A_Taxi.Web.Infrastructure.LocalResource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Get_A_Taxi.Web.ViewModels
{
    public class UserSearchVM
    {
        [Display(Name = "FirstName", ResourceType = typeof(Resource))]
        public string FirstName { get; set; }

        [Display(Name = "MiddleName", ResourceType = typeof(Resource))]
        public string MiddleName { get; set; }

        [Display(Name = "LastName", ResourceType = typeof(Resource))]
        public string LastName { get; set; }

        [Display(Name = "District", ResourceType = typeof(Resource))]
        public int DistritId { get; set; }

        [Display(Name = "Role", ResourceType = typeof(Resource))]
        public string[] SelectedRoleIds { get; set; }
    }
}