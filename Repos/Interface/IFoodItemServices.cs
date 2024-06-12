using Restaurant_WebApp.Models;

namespace Restaurant_WebApp.Repos.Interface
{
    public interface IFoodItemService
    {
        List<FoodItem> GetAllFoodItems();
        List<FoodItem> GetFoodItemsByCategory(int categoryId);
        void AddFoodItem(FoodItem foodItem, IFormFile imageFile);
        List<Category> GetAllCategories();
        Task<string> SaveImageAsync(IFormFile imageFile);

    }

}
