using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurant_WebApp.Data;
using Restaurant_WebApp.Models;
using Restaurant_WebApp.Models.ViewModels;
using Restaurant_WebApp.Repos.Interface;

namespace Restaurant_WebApp.Repos.Services
{

    public class UserServices : IUserServices
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserServices(ApplicationDbContext db, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
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

        public async Task<List<UserWithRolesVM>> GetAllUserAsync()
        {
            var userList = await _db.Users.Include(u => u.Customers).ToListAsync();
            var userWithRoles = new List<UserWithRolesVM>();

            foreach (var user in userList)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userWithRoles.Add(new UserWithRolesVM
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Roles = roles.ToList(),
                    Customers = user.Customers
                });

            }

            return userWithRoles;
        }


        public async Task<UserWithRolesVM> GetUserByIdAsync(string id)
        {
            // Fetch the user by id including related entities (e.g., Customers)
            var user = await _db.Users.Include(u => u.Customers)
                                      .FirstOrDefaultAsync(u => u.Id == id);

            var roles = await _userManager.GetRolesAsync(user);

            var userWithRolesVM = new UserWithRolesVM
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = roles.ToList(),
                Customers = user.Customers
            };


            return userWithRolesVM;
        }


        public async Task<bool> UpdateUserAsync(UserWithRolesVM modelVm)
        {
            var dbUser = await _db.Users.FindAsync(modelVm.Id);

            if (dbUser == null)
            {
                return false;
            }

            dbUser.FirstName = modelVm.FirstName;
            dbUser.LastName = modelVm.LastName;
            dbUser.Email = modelVm.Email;



            _db.Users.Update(dbUser);
            await _db.SaveChangesAsync();

            return true;
        }

        public Task<string> UploadImageFileAsync(User user)
        {
            throw new NotImplementedException();
        }


        public async Task<List<IdentityRole>> GetAllRoles()
        {
            return await _roleManager.Roles.ToListAsync();
        }


        public async Task<List<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user != null ? new List<string>(await _userManager.GetRolesAsync(user)) : new List<string>();

        }

        //public async Task<bool> UpdateUserRolesAsync(string userId, string[] selectedRoles)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user == null)
        //    {
        //        return false;
        //    }
        //    var currentRoles = await _userManager.GetRolesAsync(user);
        //    var rolesToAdd = selectedRoles.Except(currentRoles).ToArray();
        //    var rolesToRemove = currentRoles.Except(selectedRoles).ToArray();

        //    var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
        //    if (!addResult.Succeeded)
        //    {
        //        return false;
        //    }

        //    var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
        //    return removeResult.Succeeded;
        //}
    }
}
