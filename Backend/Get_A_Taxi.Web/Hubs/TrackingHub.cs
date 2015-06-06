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
    /// Subscribe for tracking assigned order's taxi and client locations
    /// There are up to to group members - the taxi and the client
    /// A group is formed, named after the order's ID
    /// Used by the taxi driver to track the client if client has enabled tracking
    /// and by the client to track taxi's position on the map
    /// </summary>
    [HubName("trackingHub")]
    [Authorize]
    public class TrackingHub: Hub
    {
        public async Task Open(int orderId)
        {
            await Groups.Add(Context.ConnectionId, orderId.ToString());
        }

        /// <summary>
        /// Taxi/client reports a location change to the other group member client/taxi
        /// </summary>
        /// <param name="orderId">The order's id</param>
        /// <param name="lat">The location's latitude</param>
        /// <param name="lon">The location's longitude</param>
        public void LocationChanged(int orderId, double lat, double lon)
        {
            Clients.OthersInGroup(orderId.ToString()).updatePeerLocation(lat, lon);
        }

        public void TaxiAssignedToOrder(int orderId, int taxiId, string plate)
        {
            Clients.OthersInGroup(orderId.ToString()).taxiAssigned(taxiId, plate);
        }

        public void OrderStatusChanged(int orderId)
        {
            Clients.OthersInGroup(orderId.ToString()).orderStatusChanged(orderId);
        }

        public Task Close(int orderId)
        {
            return Groups.Remove(Context.ConnectionId, orderId.ToString());
        }
    }
}