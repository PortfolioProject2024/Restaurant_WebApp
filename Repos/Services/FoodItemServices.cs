using Microsoft.EntityFrameworkCore;
using Restaurant_WebApp.Data;
using Restaurant_WebApp.Models;
using Restaurant_WebApp.Repos.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;

namespace Restaurant_WebApp.Repos.Services
{
    public class FoodItemService : IFoodItemService
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FoodItemService(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<FoodItem>> GetAllFoodItemsAsync()
        {
            return await _db.FoodItems.Include(f => f.Category).ToListAsync();
        }

        public async Task<List<FoodItem>> GetFoodItemsByCategoryAsync(int categoryId)
        {
            return await _db.FoodItems
                            .Include(f => f.Category)
                            .Where(f => f.CategoryId == categoryId)
                            .ToListAsync();
        }

        public async Task AddFoodItemAsync(FoodItem foodItem, IFormFile imageFile)
        {
            string imageUrl = await SaveImageAsync(imageFile);
            foodItem.ImageUrl = imageUrl;
            _db.FoodItems.Add(foodItem);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _db.Categories.ToListAsync();
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

        public async Task<FoodItem> GetFoodItemByIdAsync(int id)
        {
            return await _db.FoodItems.Include(fi => fi.Category).FirstOrDefaultAsync(fi => fi.Id == id);
        }

        public async Task UpdateFoodItemAsync(FoodItem foodItem)
        {
            _db.FoodItems.Update(foodItem);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteFoodItemAsync(int id)
        {
            var foodItem = await _db.FoodItems.FindAsync(id);
            if (foodItem != null)
            {
                _db.FoodItems.Remove(foodItem);
                await _db.SaveChangesAsync();
            }
        }
        public async Task AddCategoryAsync(Category category)
        {
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
        }
        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category != null)
            {
                _db.Categories.Remove(category);
                await _db.SaveChangesAsync();
            }
        }
        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _db.Categories.FindAsync(id);
        }
    }
}
