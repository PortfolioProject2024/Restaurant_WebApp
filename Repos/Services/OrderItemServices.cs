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

        public OrderItemServices(ApplicationDbContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _db.Orders.FindAsync(id);
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _db.Orders
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task IncludeOrderItemsAsync(Order order)
        {
            await _db.Entry(order)
                .Collection(o => o.OrderItems)
                .Query()
                .Include(oi => oi.FoodItems)
                .LoadAsync();
        }

        public async Task CreateOrderAsync(Order order)
        {
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _db.Entry(order).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order != null)
            {
                _db.Orders.Remove(order);
                await _db.SaveChangesAsync();
            }
        }

        public async Task UpdateOrderItemQuantityAsync(int orderItemId, int quantity)
        {
            var orderItem = await _db.OrderItems.FindAsync(orderItemId);
            if (orderItem != null)
            {
                orderItem.Quantity = quantity;
                await _db.SaveChangesAsync();
            }
        }
        public async Task RemoveOrderItemAsync(int orderItemId)
        {
            var orderItem = await _db.OrderItems.FindAsync(orderItemId);
            if (orderItem != null)
            {
                _db.OrderItems.Remove(orderItem);
                await _db.SaveChangesAsync();
            }
        }
        public async Task<Order> GetOrCreateActiveOrderAsync(string userId)
        {
           
            var order = await _db.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.FoodItems)
                .FirstOrDefaultAsync(o => o.UserId == userId && !o.IsCompleted);

            if (order == null)
            {
                order = new Order
                {
                    UserId = userId,
                    IsCompleted = false,
                    OrderDate = DateTime.Now
                };

                _db.Orders.Add(order);
                await _db.SaveChangesAsync();
            }

            return order;
        }

    }
}
