﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Restaurant_WebApp.Models;
using Restaurant_WebApp.Models.ViewModels;
using System.Threading.Tasks;

namespace Restaurant_WebApp.Repos.Interface
{
    public interface IUserServices
    {
        Task<List<UserWithRolesVM>> GetAllUserAsync();

        Task<bool> UpdateUserAsync(UserWithRolesVM modelVm);

        Task<UserWithRolesVM> GetUserByIdAsync(string id);

        Task<User> DeleteUserAsync(string id);

        Task<string> UploadImageFileAsync(User user);

        Task<List<SelectListItem>> GetAllRolesAsync();

        Task<List<string>> GetUserRolesAsync(string userId);

        Task<bool> CreateRoleAsync(string roleName);

        Task AssignRoleToUserAsync(string userId, string roleId);

        Task RemoveRoleFromUserAsync(string userId, string roleId);

       Task<List<User>> CustomerOrdersAsync();

        Task<User> UserWithOrdersAsync(string userId);

        Task<User> AddAddressAsync(string userId, string address,
            string postalCode, string city, string country);

        //Task<bool> UpdateUserRolesAsync(string userId, string[] selectedRoles);
        //Task<UserWithRolesVM> GetUserForEditAsync(string id);



    }
}
