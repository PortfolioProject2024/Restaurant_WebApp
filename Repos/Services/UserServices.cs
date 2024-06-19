using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var userList = await _db.Users.ToListAsync();
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
                    //Customers = user.Customers
                });

            }

            return userWithRoles;
        }


        public async Task<UserWithRolesVM> GetUserByIdAsync(string id)
        {
            // Fetch the user by id including related entities (e.g., Customers)
            var user = await _db.Users.Include(u => u.Orders)
                                      .FirstOrDefaultAsync(u => u.Id == id);

            var roles = await _userManager.GetRolesAsync(user);

            var userWithRolesVM = new UserWithRolesVM
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = roles.ToList(),
           
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
            //dbUser.Orders = modelVm.Orders;
            //dbUser.OrderItems = modelVm.OrderItems;


            _db.Users.Update(dbUser);
            await _db.SaveChangesAsync();

            return true;
        }

        public Task<string> UploadImageFileAsync(User user)
        {
            throw new NotImplementedException();
        }


        public async Task<List<SelectListItem>> GetAllRolesAsync()
        {
            var roleList = await _roleManager.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name })
                .ToListAsync();
            return roleList;
        }


        public async Task<List<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user != null ? new List<string>(await _userManager.GetRolesAsync(user)) : new List<string>();

        }

        public async Task<bool> CreateRoleAsync(string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
            {

                return false;
            }

            var newRole = new IdentityRole(roleName);


            var result = await _roleManager.CreateAsync(newRole);
            return result.Succeeded;
        }


        public async Task AssignRoleToUserAsync(string userId, string roleId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var role = await _roleManager.FindByIdAsync(roleId);

            if (user != null && role != null)
            {
                var result = await _userManager.AddToRoleAsync(user, role.Name);
                if (!result.Succeeded)
                {
                    // Handle errors if needed
                    throw new Exception($"Failed to assign role {role.Name} to user {user.UserName}");
                }
            }
        }

        public async Task RemoveRoleFromUserAsync(string userId, string roleId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var role = await _roleManager.FindByIdAsync(roleId);

            if (user != null && role != null)
            {
                var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                if (!result.Succeeded)
                {
                    // Handle errors if needed
                    throw new Exception($"Failed to remove role {role.Name} to user {user.UserName}");
                }
            }
        }


        public async Task<List<User>> CustomerOrdersAsync() 
        {
            return await _db.Users.Include(u => u.Orders).ToListAsync();
        }

        

    }
}
