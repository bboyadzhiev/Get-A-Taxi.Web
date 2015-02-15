namespace Get_A_Taxi.Web.Infrastructure.Services.Contracts
{
    using Get_A_Taxi.Models;
    using Get_A_Taxi.Web.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IOperatorService
    {
        IQueryable<Order> OrdersByWaitingTime();

        IQueryable<Order> OrdersByCompletionTime();

        IQueryable<Order> OrdersByCompletionNearLocation(double lat, double lon);
    }
}
