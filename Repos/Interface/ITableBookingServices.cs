using Restaurant_WebApp.Models;

namespace Restaurant_WebApp.Repos.Interface
{
    public interface ITableBookingServices
    {
        Task<IEnumerable<TableBooking>> GetAllBookingsAsync(int Id);

        Task AddTableBookingAsync(TableBooking tableBooking);

        Task<TableBooking> DeleteTableBookingAsync(int Id);

        //Task<TableBooking> GetBookingAsync(int Id);

        Task<TableBooking> ConfirmationMessageAsync(string phone, string name);

        Task<TableBooking> GetBookingByIdAsync(int Id);

        Task<bool> UpdateBookingAsync(TableBooking tableBooking);

    }
}
