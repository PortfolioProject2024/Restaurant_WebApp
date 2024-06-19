using Restaurant_WebApp.Models;

namespace Restaurant_WebApp.Repos.Interface
{
    public interface IContactService 
    {
        Task<List<Contact>> GetAllMessagesAsync();

        Task<Contact> DeleteMessageAsync(int id);

        Task<Contact> GetMessageByIdAsync(int id);

        Task AddMessageAsync(Contact contact);


    }
}
