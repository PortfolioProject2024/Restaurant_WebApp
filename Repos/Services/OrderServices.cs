using Microsoft.EntityFrameworkCore;
using Restaurant_WebApp.Data;
using Restaurant_WebApp.Models;
using Restaurant_WebApp.Repos.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant_WebApp.Repos.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly ApplicationDbContext _db;

        public OrderServices(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _db.Orders
                            .Include(o => o.User)
                            .Include(o => o.OrderItems)
                                .ThenInclude(oi => oi.FoodItems)
                            .ToListAsync();
        }

        public async Task<OrderItem> GetOrderItemByIdAsync(int id)
        {
            return await _db.OrderItems
                            .Include(oi => oi.Order)
                                .ThenInclude(o => o.User)
                            .Include(oi => oi.FoodItems)
                            .FirstOrDefaultAsync(oi => oi.Id == id);
        }

        public async Task UpdateOrderItemAsync(OrderItem orderItem)
        {
            _db.Update(orderItem);
            await _db.SaveChangesAsync();
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

        public bool OrderItemExists(int id)
        {
            return _db.OrderItems.Any(oi => oi.Id == id);
        }
        //-------------------------------------------
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
        public async Task<Order> GetOrCreateActiveOrderAsync(string userId)
        {
            var activeOrder = await _db.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.UserId == userId && !o.IsCompleted);

            if (activeOrder == null)
            {
                activeOrder = new Order
                {
                    UserId = userId,
                    IsCompleted = false
                };
                _db.Orders.Add(activeOrder);
                await _db.SaveChangesAsync();
            }

            return activeOrder;
        }
    }
}
