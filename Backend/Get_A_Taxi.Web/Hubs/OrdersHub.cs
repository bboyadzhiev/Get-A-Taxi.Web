namespace Get_A_Taxi.Web.Hubs
{
    using AutoMapper.QueryableExtensions;
    using Get_A_Taxi.Web.Infrastructure.Services.Hubs.HubServices;
    using Get_A_Taxi.Web.ViewModels;
    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;
    using System.Threading.Tasks;
    using System.Linq;

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
            var ordersDisplayVMList = result.Project().To<OrderDetailsVM>().ToList();

            await Groups.Add(Context.ConnectionId, districtGroup);
            Clients.Caller.updateOrders(ordersDisplayVMList);
        }

        public Task Close(int districtId, string operatorId)
        {
            return Groups.Remove(Context.ConnectionId, districtId.ToString());
        }
    }
}