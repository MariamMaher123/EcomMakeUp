using EcomMakeUp.Models;

namespace EcomMakeUp.Servies
{
    public interface IOrderServies
    {
        Task<object> CreateOrder(Order order);
        Task<object> UpdateOrder(Order order);
        string DeleteOrder(Order order);
        Task<object> GetOrderById(int orderId);
        Task<IEnumerable<object>> GetAllOrders();
        Task<IEnumerable<object>> GetOrdersByIdUser (string userId);
        Task<IEnumerable<object>> GetWaitingOrdersByIdUser(string userId);
        Task<IEnumerable<object>> GetBuyingOrdersByIdUser(string userId);

    }
}
