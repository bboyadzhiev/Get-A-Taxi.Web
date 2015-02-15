using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Get_A_Taxi.Web.ViewModels;

namespace Get_A_Taxi.Web.Infrastructure.Services.Hubs
{
    public class OrderServiceEventArgs: EventArgs
    {
        public OrderVM Order { get; private set; }
        public OrderServiceEventArgs(OrderVM order)
        {
            this.Order = order;
        }
    }
}
