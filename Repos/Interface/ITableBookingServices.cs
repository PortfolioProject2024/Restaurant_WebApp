using Restaurant_WebApp.Models;

namespace Restaurant_WebApp.Repos.Interface
{
    public interface ITableBookingServices
    {
        Task<TableBooking> AddTableBookingAsync(TableBooking tableBooking, int customerId);

        Task<IEnumerable<TableBooking>> GetCustomerBookingsAsync(int customerId, string userId);

    }
}
