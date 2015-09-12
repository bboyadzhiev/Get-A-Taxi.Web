using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using AutoMapper.QueryableExtensions;
using Get_A_Taxi.Web.Infrastructure.Services.HubServices;
using Get_A_Taxi.Web.Models;

namespace Get_A_Taxi.Web.Hubs
{
    /// <summary>
    /// Subscribe operators in the district about all taxies' status changes
    /// </summary>
    [Authorize]
    [HubName("taxiesHub")]
    public class TaxiesHub : Hub
    {
         private ITaxiesHubService _service;
         public TaxiesHub(ITaxiesHubService taxiService)
         {
             this._service = taxiService;
         }

         public async Task Open(int districtId)
         {
             var districtGroup = districtId.ToString();
             await Groups.Add(Context.ConnectionId, districtGroup);

             var result = this._service.AllTaxies().Where(o => o.District.DistrictId == districtId);
             result = this._service.OnDuty(result);
             var onDutyTaxiesList = result.Project().To<TaxiDetailsDTO>().ToList();
             
             // Return currently operating taxies
             Clients.Caller.populateTaxies(onDutyTaxiesList);
         }

         public Task Close(int districtId)
         {
             return Groups.Remove(Context.ConnectionId, districtId.ToString());
         }
    }
}