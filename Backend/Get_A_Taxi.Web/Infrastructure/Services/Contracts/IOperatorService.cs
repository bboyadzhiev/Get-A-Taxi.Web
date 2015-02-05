namespace Get_A_Taxi.Web.Infrastructure.Services.Contracts
{
    using Get_A_Taxi.Models;
    using System.Collections.Generic;
using System.Linq;

    interface IOperatorService
    {
        IQueryable<Order> OrdersByWaitingTime();

        IQueryable<Order> OrdersByCompletionTime();

        IQueryable<Order> OrdersByCompletionNearLocation(double lat, double lon);
    }
}
