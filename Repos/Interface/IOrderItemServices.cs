using Restaurant_WebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant_WebApp.Repos.Interface
{
    public interface IOrderItemServices
    {
        Task<OrderItem> GetOrderItemByIdAsync(int id);
        Task<List<OrderItem>> GetOrderItemsAsync();
        Task<List<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId);
        Task<OrderItem> CreateOrderItemAsync(OrderItem orderItem);
        Task<OrderItem> UpdateOrderItemAsync(OrderItem orderItem);
        Task DeleteOrderItemAsync(int id);
    }
}
