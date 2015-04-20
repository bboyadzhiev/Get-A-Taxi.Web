using AutoMapper.QueryableExtensions;
using Get_A_Taxi.Models;
using Get_A_Taxi.Web.Infrastructure.Services.Hubs.HubServices;
using Get_A_Taxi.Web.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Get_A_Taxi.Web.Hubs
{
    [HubName("taxiesHub")]
    public class TaxiesHub : Hub
    {
         private ITaxiesHubService _service;
         public TaxiesHub(ITaxiesHubService taxiService)
         {
             this._service = taxiService;
         }

         public async Task Open(int districtId, string operatorId)
         {
             var districtGroup = districtId.ToString();

             var result = this._service.AllTaxies().Where(o => o.District.DistrictId == districtId);
             result = this._service.OnDuty(result);
             var onDutyaxiesList = result.Project().To<TaxiDetailsDTO>().ToList();

             await Groups.Add(Context.ConnectionId, districtGroup);
             Clients.Caller.populateTaxies(onDutyaxiesList);
         }

         public Task Close(int districtId, string operatorId)
         {
             return Groups.Remove(Context.ConnectionId, districtId.ToString());
         }
    }
}