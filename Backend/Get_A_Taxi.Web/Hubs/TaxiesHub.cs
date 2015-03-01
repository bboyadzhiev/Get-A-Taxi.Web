using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Get_A_Taxi.Web.Hubs
{
     [HubName("taxiesHub")]
    public class TaxiesHub : Hub
    {

    }
}