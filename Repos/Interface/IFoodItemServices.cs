using Restaurant_WebApp.Models;

namespace Restaurant_WebApp.Repos.Interface
{
    public interface IFoodItemService
    {
        Task<FoodItem> CreateFoodItem(FoodItem foodItem);
        Task<FoodItem> GetFoodItem(int id);
        Task<IEnumerable<FoodItem>> GetFoodItems();
        Task UpdateFoodItem(FoodItem foodItem);
        Task DeleteFoodItem(int id);
    }

}
