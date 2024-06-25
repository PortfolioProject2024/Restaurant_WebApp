using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurant_WebApp.Data;
using Restaurant_WebApp.Models;
using Restaurant_WebApp.Repos.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant_WebApp.Repos.Services
{
    public class OrderItemServices : IOrderItemServices
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;

        public async Task<OrderItem> GetOrderItemByIdAsync(int id)
        {
            return await _db.OrderItems.FindAsync(id);
        }

        public async Task<List<OrderItem>> GetOrderItemsAsync()
        {
            return await _db.OrderItems.ToListAsync();
        }

        public async Task<List<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId)
        {
            return await _db.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<OrderItem> CreateOrderItemAsync(OrderItem orderItem)
        {
            _db.OrderItems.Add(orderItem);
            await _db.SaveChangesAsync();
            return orderItem;
        }

        public async Task<OrderItem> UpdateOrderItemAsync(OrderItem orderItem)
        {
            _db.Entry(orderItem).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return orderItem;
        }

        public async Task DeleteOrderItemAsync(int id)
        {
            var orderItem = await _db.OrderItems.FindAsync(id);
            if (orderItem != null)
            {
                _db.OrderItems.Remove(orderItem);
                await _db.SaveChangesAsync();
            }
        }


    }
}
