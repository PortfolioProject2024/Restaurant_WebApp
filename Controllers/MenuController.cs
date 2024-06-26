using Microsoft.AspNetCore.Mvc;
using Restaurant_WebApp.Models;
using Restaurant_WebApp.Repos.Interface;
using Restaurant_WebApp.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using System.IO;
using System;

namespace Restaurant_WebApp.Controllers
{
    public class MenuController : Controller
    {
        private readonly IFoodItemServices _foodItemServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MenuController(IFoodItemServices foodItemServices, IWebHostEnvironment webHostEnvironment)
        {
            _foodItemServices = foodItemServices;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new MenuViewModel
            {
                FoodItems = await _foodItemServices.GetAllFoodItemsAsync(),
                Categories = await _foodItemServices.GetAllCategoriesAsync()
            };

            ViewBag.ShowEditDelete = true;
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> FilterByCategory(int categoryId)
        {
            List<FoodItem> foodItems = categoryId == 0
                ? await _foodItemServices.GetAllFoodItemsAsync()
                : await _foodItemServices.GetFoodItemsByCategoryAsync(categoryId);

            ViewBag.ShowEditDelete = true;
            return PartialView("_FoodItemsPartial", foodItems);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new FoodItemViewModel
            {
                Categories = await _foodItemServices.GetAllCategoriesAsync()
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FoodItemViewModel viewModel, IFormFile ImageFile)
        {
            var foodItem = new FoodItem
            {

                FoodName = viewModel.FoodName,
                FoodDescription = viewModel.FoodDescription,
                FoodPrice = viewModel.FoodPrice,
                CategoryId = viewModel.CategoryId,
                ImageUrl = viewModel.ImageUrl,
            };

            await _foodItemServices.AddFoodItemAsync(foodItem, ImageFile);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var foodItem = await _foodItemServices.GetFoodItemByIdAsync(id);
            if (foodItem == null)
            {
                return NotFound();
            }

            var viewModel = new FoodItemViewModel
            {
                Id = id,
                FoodName = foodItem.FoodName,
                FoodDescription = foodItem.FoodDescription,
                FoodPrice = foodItem.FoodPrice,
                CategoryId = foodItem.CategoryId,
                Categories = await _foodItemServices.GetAllCategoriesAsync(),
                ImageUrl = foodItem.ImageUrl
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FoodItemViewModel viewModel, IFormFile ImageFile)
        {
            var foodItem = await _foodItemServices.GetFoodItemByIdAsync(viewModel.Id);
            if (foodItem == null)
            {
                return NotFound();
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {
                string fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(ImageFile.FileName)}";
                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Files", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                if (!string.IsNullOrEmpty(foodItem.ImageUrl))
                {
                    string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "Files", foodItem.ImageUrl);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                foodItem.ImageUrl = fileName;
            }

            foodItem.FoodName = viewModel.FoodName;
            foodItem.FoodDescription = viewModel.FoodDescription;
            foodItem.FoodPrice = viewModel.FoodPrice;
            foodItem.CategoryId = viewModel.CategoryId;

            await _foodItemServices.UpdateFoodItemAsync(foodItem);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var foodItem = await _foodItemServices.GetFoodItemByIdAsync(id);
            if (foodItem == null)
            {
                return NotFound();
            }

            return View(foodItem);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var foodItem = await _foodItemServices.GetFoodItemByIdAsync(id);
            if (foodItem == null)
            {
                return NotFound();
            }

            await _foodItemServices.DeleteFoodItemAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var foodItem = await _foodItemServices.GetFoodItemByIdAsync(id);
            if (foodItem == null)
            {
                return NotFound();
            }

            return View(foodItem);
        }
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                await _foodItemServices.AddCategoryAsync(category);
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Categories()
        {
            var categories = await _foodItemServices.GetAllCategoriesAsync();
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _foodItemServices.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("DeleteCategoryConfirmed")]
        public async Task<IActionResult> DeleteCategoryConfirmed(int id)
        {
            await _foodItemServices.DeleteCategoryAsync(id);
            return RedirectToAction("Categories");
        }


    }
}