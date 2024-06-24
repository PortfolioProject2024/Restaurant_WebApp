using Restaurant_WebApp.Models;

namespace Restaurant_WebApp.Repos.Interface
{
    public interface IOrderServices
    {
        Task<List<Order>> GetAllOrdersAsync();
        Task<OrderItem> GetOrderItemByIdAsync(int id);
        Task UpdateOrderItemAsync(OrderItem orderItem);
        Task DeleteOrderItemAsync(int id);
        bool OrderItemExists(int id);
      

        //------------------------------------
        Task<List<Order>> GetOrdersByUserIdAsync(string userId);
        Task IncludeOrderItemsAsync(Order order);
        Task CreateOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
        Task<Order> GetOrderByIdAsync(int id);
        Task<Order> GetOrCreateActiveOrderAsync(string userId);
    }
}
