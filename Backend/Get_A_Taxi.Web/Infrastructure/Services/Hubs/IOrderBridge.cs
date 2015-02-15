using Get_A_Taxi.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Get_A_Taxi.Web.Infrastructure.Services.Hubs
{
    public interface IOrderBridge
    {
        void AddOrder(int orderId);
        void CancelOrder(int orderId);

        event EventHandler<int> OrderAddedEvent;
        event EventHandler<int> OrderCancelledEvent;
    }
}
