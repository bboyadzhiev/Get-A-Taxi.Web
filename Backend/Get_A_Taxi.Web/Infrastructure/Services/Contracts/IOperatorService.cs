using Get_A_Taxi.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Get_A_Taxi.Web.Infrastructure.Services.Contracts
{
    interface IOperatorService
    {
        IList<OrderViewModel> GetOrderVMList(int count);

    }
}
