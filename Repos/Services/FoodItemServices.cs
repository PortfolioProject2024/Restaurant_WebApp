using Microsoft.EntityFrameworkCore;
using Restaurant_WebApp.Data;
using Restaurant_WebApp.Models;
using Restaurant_WebApp.Repos.Interface;

namespace Restaurant_WebApp.Repos.Services
{
    public class FoodItemServices : IFoodItemServices
    {
        private readonly ApplicationDbContext _db;

        public FoodItemServices(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<FoodItem> CreateFoodItem(FoodItem foodItem)
        {
            _db.FoodItems.Add(foodItem);
            await _db.SaveChangesAsync();
            return foodItem;
        }

        public async Task<FoodItem> GetFoodItem(int id)
        {
            return await _db.FoodItems.FindAsync(id);
        }

        public async Task<IEnumerable<FoodItem>> GetFoodItems()
        {
            return await _db.FoodItems.ToListAsync();
        }

        public async Task UpdateFoodItem(FoodItem foodItem)
        {
            _db.Entry(foodItem).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteFoodItem(int id)
        {
            var foodItem = await GetFoodItem(id);
            if (foodItem != null)
            {
                _db.FoodItems.Remove(foodItem);
                await _db.SaveChangesAsync();
            }
        }
    }
}
