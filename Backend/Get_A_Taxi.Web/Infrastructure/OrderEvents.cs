using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Get_A_Taxi.Web.Infrastructure
{
    /// <summary>
    /// Temporary solution, replacement for OrdersBridge bug problem
    /// </summary>
    [Obsolete("Use OrdersBridge instead", true)]
    public static class OrdersEvents
    {
        public static event EventHandler<int> OrderAddedEvent;
        public static event EventHandler<int> OrderCancelledEvent;
        public static void AddOrder(int orderId)
        {
            if (OrderAddedEvent != null)
            {
                OrderAddedEvent(null, orderId);
            }
        }

        public static void CancelOrder(int orderId)
        {
            if (OrderCancelledEvent != null)
            {
                OrderCancelledEvent(null, orderId);
            }
        }

    }
}