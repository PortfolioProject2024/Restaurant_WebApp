using Microsoft.EntityFrameworkCore;
using Restaurant_WebApp.Data;
using Restaurant_WebApp.Models;
using Restaurant_WebApp.Repos.Interface;

namespace Restaurant_WebApp.Repos.Services
{
    public class FoodItemService : IFoodItemService
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FoodItemService(ApplicationDbContext db , IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;   
        }

        public List<FoodItem> GetAllFoodItems()
        {
            return _db.FoodItems.Include(f => f.Category).ToList();
        }

        public List<FoodItem> GetFoodItemsByCategory(int categoryId)
        {
            return _db.FoodItems
                            .Include(f => f.Category)
                            .Where(f => f.CategoryId == categoryId)
                            .ToList();
        }


        public void AddFoodItem(FoodItem foodItem, IFormFile imageFile)
        {
            string imageUrl = SaveImageAsync(imageFile).Result; 
            foodItem.ImageUrl = imageUrl; 
            _db.FoodItems.Add(foodItem); 
            _db.SaveChanges(); 
        }


        public List<Category> GetAllCategories()
        {
            return _db.Categories.ToList();
        }
        public async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            var fileName = $"{Guid.NewGuid()}.jpg";
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Files", fileName);

            var directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return fileName;
        }

        public FoodItem GetFoodItemById(int id)
        {
            return _db.FoodItems.Include(fi => fi.Category).FirstOrDefault(fi => fi.Id == id);
        }

        public void UpdateFoodItem(FoodItem foodItem)
        {
            _db.FoodItems.Update(foodItem);
            _db.SaveChanges();
        }
        public void DeleteFoodItem(int id)
        {
            var foodItem = _db.FoodItems.Find(id);
            if (foodItem != null)
            {
                _db.FoodItems.Remove(foodItem);
                _db.SaveChanges();
            }
        }
    }
}
