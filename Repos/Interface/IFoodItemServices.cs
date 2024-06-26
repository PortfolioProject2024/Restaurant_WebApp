using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Restaurant_WebApp.Models;

namespace Restaurant_WebApp.Repos.Interface
{
    public interface IFoodItemServices
    {
        Task<List<FoodItem>> GetAllFoodItemsAsync();
        Task<List<FoodItem>> GetFoodItemsByCategoryAsync(int categoryId);
        Task AddFoodItemAsync(FoodItem foodItem, IFormFile imageFile);
        Task<List<Category>> GetAllCategoriesAsync();
        Task<string> SaveImageAsync(IFormFile imageFile);
        Task<FoodItem> GetFoodItemByIdAsync(int id);
        Task UpdateFoodItemAsync(FoodItem foodItem);
        Task DeleteFoodItemAsync(int id);
        Task AddCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
        Task<Category> GetCategoryByIdAsync(int id);
        Task IncludeFoodItemsAsync(Order order);
    }
}
