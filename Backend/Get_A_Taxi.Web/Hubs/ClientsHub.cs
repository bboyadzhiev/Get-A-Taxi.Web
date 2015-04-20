using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Get_A_Taxi.Web.Hubs
{
    /// <summary>
    /// Mobile clients hub
    /// hub group is every client Id
    /// </summary>
    [HubName("clientsHub")]
    public class ClientsHub: Hub
    {
        public ClientsHub()
        {

        }

        /// <summary>
        /// When client connects a new 
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task Open(string clientId)
        {
            await Groups.Add(Context.ConnectionId, clientId);
            Clients.Caller.ok(clientId);
        }
    }
}