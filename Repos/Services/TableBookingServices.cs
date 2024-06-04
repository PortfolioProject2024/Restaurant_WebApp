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

        public async Task AddTableBookingAsync(TableBooking tableBooking)
        {
            _db.TableBookings.Add(tableBooking);
            await _db.SaveChangesAsync();

        }

        public async Task<TableBooking> DeleteTableBookingAsync(int Id)
        {
            var del = await _db.TableBookings.FindAsync(Id);
            
            _db.TableBookings.Remove(del);
            await _db.SaveChangesAsync();
            return del;

        }

        public async Task<TableBooking> GetBookingAsync(int tableId)
        {
            var byId = await _db.TableBookings.FirstOrDefaultAsync(x => x.Id == tableId);
            return byId;
        }

        public async Task<IEnumerable<TableBooking>> GetAllBookingsAsync(int Id)
        {
            return await _db.TableBookings.ToListAsync();
        }
    }
}
