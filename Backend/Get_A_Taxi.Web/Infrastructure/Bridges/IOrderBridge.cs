using Get_A_Taxi.Web.Models;

namespace Get_A_Taxi.Web.Infrastructure.Bridges
{
    public interface IOrderBridge
    {
        void AddOrder(OrderDetailsDTO order, int districtId);
        void CancelOrder(int orderId, int districtId);
        void AssignOrder(int orderId, int taxiId, int districtId);
        void UpdateOrder(OrderDetailsDTO order, int districtId);

    }
}
