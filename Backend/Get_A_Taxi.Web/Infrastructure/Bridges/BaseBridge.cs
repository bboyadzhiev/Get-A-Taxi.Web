using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Get_A_Taxi.Web.Infrastructure.Bridges
{
    public class BaseBridge
    {
        protected IHubConnectionContext<dynamic> Clients { get; set; }
        public BaseBridge(IHubConnectionContext<dynamic> clients)
        {
            if (clients == null)
            {
                throw new ArgumentNullException("clients");
            }

            Clients = clients;
        }
    }
}