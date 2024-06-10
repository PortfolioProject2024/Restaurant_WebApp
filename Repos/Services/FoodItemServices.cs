using Microsoft.EntityFrameworkCore;
using Restaurant_WebApp.Data;
using Restaurant_WebApp.Models;
using Restaurant_WebApp.Repos.Interface;

namespace Restaurant_WebApp.Repos.Services
{
    public class FoodItemService : IFoodItemService
    {
        private readonly ApplicationDbContext _db;

        public FoodItemService(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<FoodItem> GetAllFoodItems()
        {
            return _db.FoodItems.Include(f => f.Category).ToList();
        }

        public List<FoodItem> GetFoodItemsByCategory(int categoryId)
        {
            return _db.FoodItems.Where(f => f.CategoryId == categoryId)
                                 .Include(f => f.Category)
                                 .ToList();
        }

        public void AddFoodItem(FoodItem foodItem)
        {
            _db.FoodItems.Add(foodItem);
            _db.SaveChanges();
        }

        public List<Category> GetAllCategories()
        {
            return _db.Categories.ToList();
        }
    }
}
