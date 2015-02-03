using Get_A_Taxi.Data;
using Get_A_Taxi.Web.Infrastructure.Services.Base;
using Get_A_Taxi.Web.Infrastructure.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Get_A_Taxi.Web.Infrastructure.Services
{
    public class OperatorService : BaseService, IOperatorService
    {
        public OperatorService(IGetATaxiData data)
            :base(data)
        {
        }

        public IList<ViewModels.OrderDetailsVM> GetOrderVMList(int count)
        {
            throw new NotImplementedException();
        }
    }
}