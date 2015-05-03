using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Get_A_Taxi.Web.Hubs
{

    //TODO Obsolete

    /// <summary>
    /// Mobile clients hub
    /// hub group is client Id
    /// </summary>
    [HubName("clientsHub")]
    [Obsolete]
    public class ClientsHub: Hub
    {

        /// <summary>
        /// Add SignalR subscribers to clients status changes
        /// </summary>
        /// <param name="clientId"></param>
        public async Task Open(string clientId)
        {
            await Groups.Add(Context.ConnectionId, clientId);
            Clients.Caller.ok(clientId);
        }

        public Task Close(string clientId)
        {
            return Groups.Remove(Context.ConnectionId, clientId.ToString());
        }
    }
}