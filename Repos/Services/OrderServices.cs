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

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _db.Orders
                            .Include(o => o.User)
                            .Include(o => o.OrderItems)
                                .ThenInclude(oi => oi.FoodItems)
                            .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _db.Update(order);
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

        public bool OrderExists(int id)
        {
            return _db.Orders.Any(o => o.Id == id);
        }
    }
}
