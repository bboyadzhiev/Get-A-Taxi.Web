namespace Get_A_Taxi.Web.Infrastructure.Services.Contracts
{
    using Get_A_Taxi.Web.ViewModels;
    using System.Collections.Generic;

    public interface IOrdersListService
    {
        IList<OrderDetailsVM> GetOrderVMList(int count);
    }
}
