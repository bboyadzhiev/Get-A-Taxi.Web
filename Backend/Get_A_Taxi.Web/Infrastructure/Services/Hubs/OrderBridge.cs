using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Get_A_Taxi.Web.Infrastructure.Services.Hubs
{
    public class OrderBridge : IOrderBridge
    {
        public event EventHandler<int> OrderAddedEvent;
        public event EventHandler<int> OrderCancelledEvent;
        public void AddOrder(int orderId)
        {
            if (OrderAddedEvent != null)
            {
                OrderAddedEvent(this, orderId);
            }
        }

        public void CancelOrder(int orderId)
        {
            if (OrderCancelledEvent != null)
            {
                OrderCancelledEvent(this, orderId);
            }
        }
    }
}