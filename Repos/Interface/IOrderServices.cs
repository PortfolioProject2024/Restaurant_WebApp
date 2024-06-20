using Restaurant_WebApp.Models;

namespace Restaurant_WebApp.Repos.Interface
{
    public interface IOrderServices
    {
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<List<Order>> GetAllOrdersAsync();
        Task CreateOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int orderId);
    }
}
