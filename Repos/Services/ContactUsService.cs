using Microsoft.EntityFrameworkCore;
using Restaurant_WebApp.Data;
using Restaurant_WebApp.Models;
using Restaurant_WebApp.Repos.Interface;

namespace Restaurant_WebApp.Repos.Services
{
    public class ContactUsService : IContactUsService
    {
        private readonly ApplicationDbContext _db;

        public ContactUsService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddMessageAsync(ContactUs contact)
        {
            _db.Contacts.Add(contact);
            await _db.SaveChangesAsync();
        }

        public async Task<ContactUs> DeleteMessageAsync(int id)
        {
            var del = await _db.Contacts.FindAsync(id);

            _db.Contacts.Remove(del);
            await _db.SaveChangesAsync();
            return del;
        }

        public async Task<List<ContactUs>> GetAllMessagesAsync()
        {
            return await _db.Contacts.ToListAsync();
        }

        public async Task<ContactUs> GetMessageByIdAsync(int id)
        {
            return await _db.Contacts.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
