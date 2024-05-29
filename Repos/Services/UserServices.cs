using Restaurant_WebApp.Models;
using Restaurant_WebApp.Repos.Interface;

namespace Restaurant_WebApp.Repos.Services
{
    public class UserServices : IUserServices
    {
        public Task<User> DeleteUserAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAllUserAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAllUsersInRoleAsync(string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<string> UploadImageFileAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
