using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Restaurant_WebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant_WebApp.Repos.Interface
{
    public interface IOrderItemServices
    {
        Task<List<Order>> GetOrdersByUserIdAsync(string userId);
        Task IncludeOrderItemsAsync(Order order);
        Task CreateOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
        Task<Order> GetOrderByIdAsync(int id);
        Task<Order> GetOrCreateActiveOrderAsync(string userId);
        Task UpdateOrderItemQuantityAsync(int orderItemId,int quantity);
        Task RemoveOrderItemAsync(int orderItemId);
    }
}
