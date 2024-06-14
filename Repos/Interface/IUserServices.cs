using Restaurant_WebApp.Models;
using System.Threading.Tasks;

namespace Restaurant_WebApp.Repos.Interface
{
    public interface IUserServices
    {
        Task<List<User>> GetAllUserAsync();

        Task<bool> UpdateUserAsync(User user);

        Task<User> GetUserByIdAsync(string id);

        Task<User> DeleteUserAsync(string id);

        Task<string> UploadImageFileAsync(User user);

        Task<List<User>> GetAllUsersInRoleAsync(string roleName);

      


    }
}
