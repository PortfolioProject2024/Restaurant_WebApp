using Microsoft.EntityFrameworkCore;
using Restaurant_WebApp.Data;
using Restaurant_WebApp.Models;
using Restaurant_WebApp.Repos.Interface;

namespace Restaurant_WebApp.Repos.Services
{
    public class TableBookingServices : ITableBookingServices
    {
        private readonly ApplicationDbContext _db;

        public TableBookingServices(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<TableBooking> AddTableBookingAsync(TableBooking tableBooking, int customerId)
        {
             tableBooking.CustomerId = customerId;

            _db.TableBookings.Add(tableBooking);
            await _db.SaveChangesAsync();
            return tableBooking;
                               
        }

        public async Task<IEnumerable<TableBooking>> GetCustomerBookingsAsync(int customerId, string userId)
        {
            return await _db.TableBookings.Where(tb => tb.CustomerId == customerId && tb.UserId == userId)
                .ToListAsync();
        }
    }
}
