using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurant_WebApp.Data;
using Restaurant_WebApp.Models;
using Restaurant_WebApp.Repos.Interface;

namespace Restaurant_WebApp.Repos.Services
{

    public class UserServices : IUserServices
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;

        public UserServices(ApplicationDbContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<User> DeleteUserAsync(string id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null)
            {
                return null;
            }
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();

            return user;

        }

        public async Task<List<User>> GetAllUserAsync()
        {
            return await _db.Users.Include(u => u.Customers).ToListAsync();
        }

        public async Task<List<User>> GetAllUsersInRoleAsync(string roleName)
        {
            var userInRoles = await _userManager.GetUsersInRoleAsync(roleName);
            return userInRoles.ToList();
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            var user = await _db.Users.Include(u => u.Customers)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }
            return user;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                var userInDb = await _db.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

                if (userInDb == null)
                {
                    return false;
                }

                userInDb.FirstName = user.FirstName;
                userInDb.LastName = user.LastName;
                userInDb.Email = user.Email;
                userInDb.PhoneNumber = user.PhoneNumber;
                userInDb.UserName = user.UserName;
                userInDb.Customers = user.Customers;

                await _db.SaveChangesAsync();   
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public Task<string> UploadImageFileAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
