using Microsoft.EntityFrameworkCore;
using Restaurant_WebApp.Data;
using Restaurant_WebApp.Models;
using Restaurant_WebApp.Repos.Interface;

namespace Restaurant_WebApp.Repos.Services
{
    public class OrderService : IOrderServices
    {
        private readonly ApplicationDbContext _db;

        public OrderService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _db.Orders.FindAsync(orderId);
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _db.Orders.ToListAsync();
        }

        public async Task CreateOrderAsync(Order order)
        {
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _db.Orders.Update(order);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            var order = await _db.Orders.FindAsync(orderId);
            if (order != null)
            {
                _db.Orders.Remove(order);
                await _db.SaveChangesAsync();
            }
        }
    }
}


