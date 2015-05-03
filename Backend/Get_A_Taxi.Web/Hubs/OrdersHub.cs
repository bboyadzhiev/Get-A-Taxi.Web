namespace Get_A_Taxi.Web.Hubs
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;
    using AutoMapper.QueryableExtensions;
    using Get_A_Taxi.Web.Infrastructure.Services.Hubs.HubServices;
    using Get_A_Taxi.Web.Models;

    /// <summary>
    /// Subscribes operators and taxies to the new orders that are being placed in the district
    /// The hub's group is the district id
    /// </summary>
    [HubName("ordersHub")]
    [Authorize]
    public class OrdersHub : Hub
    {
        private IOrdersHubService _service;

        public OrdersHub(IOrdersHubService service)
        {
            this._service = service;

        }

        /// <summary>
        /// Subscribe to the hub and fetch all the current orders
        /// </summary>
        /// <param name="districtId">The id of the distrit</param>
        /// <returns></returns>
        public async Task Open(int districtId)
        {
            var districtGroup = districtId.ToString();

            var result = this._service.AllOrders().Where(o => o.District.DistrictId == districtId);
            result = this._service.GetUnfinished(result);
            var ordersDisplayDTOList = result.Project().To<OrderDetailsDTO>().ToList();

            await Groups.Add(Context.ConnectionId, districtGroup);
            Clients.Caller.updateOrders(ordersDisplayDTOList);
        }

        public Task Close(int districtId)
        {
            return Groups.Remove(Context.ConnectionId, districtId.ToString());
        }
    }
}