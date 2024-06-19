using Microsoft.EntityFrameworkCore;
using Restaurant_WebApp.Data;
using Restaurant_WebApp.Models;
using Restaurant_WebApp.Repos.Interface;

namespace Restaurant_WebApp.Repos.Services
{
    public class ContactService : IContactService
    {
        private readonly ApplicationDbContext _db;

        public ContactService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddMessageAsync(Contact contact)
        {
            _db.Contacts.Add(contact);
            await _db.SaveChangesAsync();
        }

        public async Task<Contact> DeleteMessageAsync(int id)
        {
            var del = await _db.Contacts.FindAsync(id);

            _db.Contacts.Remove(del);
            await _db.SaveChangesAsync();
            return del;
        }

        public async Task<List<Contact>> GetAllMessagesAsync()
        {
            return await _db.Contacts.ToListAsync();
        }

        public async Task<Contact> GetMessageByIdAsync(int id)
        {
            return await _db.Contacts.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
