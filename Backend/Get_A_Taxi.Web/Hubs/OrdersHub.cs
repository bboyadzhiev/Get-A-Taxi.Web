namespace Get_A_Taxi.Web.Hubs
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;
    using AutoMapper.QueryableExtensions;
    using Get_A_Taxi.Web.Infrastructure.Services.Hubs.HubServices;
    using Get_A_Taxi.Web.Models;

    [HubName("ordersHub")]
    public class OrdersHub : Hub
    {
        private IOrdersHubService _service;

        public OrdersHub(IOrdersHubService service)
        {
            this._service = service;

        }

        public async Task Open(int districtId, string operatorId)
        {
            var districtGroup = districtId.ToString();

            var result = this._service.AllOrders().Where(o => o.District.DistrictId == districtId);
            result = this._service.GetUnfinished(result);
            var ordersDisplayDTOList = result.Project().To<OrderDetailsDTO>().ToList();

            await Groups.Add(Context.ConnectionId, districtGroup);
            Clients.Caller.updateOrders(ordersDisplayDTOList);
        }

        public Task Close(int districtId, string operatorId)
        {
            return Groups.Remove(Context.ConnectionId, districtId.ToString());
        }
    }
}