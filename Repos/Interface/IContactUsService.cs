using Restaurant_WebApp.Models;

namespace Restaurant_WebApp.Repos.Interface
{
    public interface IContactUsService 
    {
        Task<List<ContactUs>> GetAllMessagesAsync();

        Task<ContactUs> DeleteMessageAsync(int id);

        Task<ContactUs> GetMessageByIdAsync(int id);

        Task AddMessageAsync(ContactUs contact);


    }
}
