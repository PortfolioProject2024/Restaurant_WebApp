using Microsoft.AspNetCore.Mvc;
using Restaurant_WebApp.Models;
using Restaurant_WebApp.Repos.Interface;
using Restaurant_WebApp.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Restaurant_WebApp.Migrations;

namespace Restaurant_WebApp.Controllers
{
    public class MenuController : Controller
    {
        private readonly IFoodItemService _foodItemService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MenuController(IFoodItemService foodItemService, IWebHostEnvironment webHostEnvironment)
        {
            _foodItemService = foodItemService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index( )
        {
            var viewModel = new MenuViewModel
            {
                FoodItems = _foodItemService.GetAllFoodItems(),
                Categories = _foodItemService.GetAllCategories()
            };

          
           
            ViewBag.ShowEditDelete = true;
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult FilterByCategory(int categoryId)
        {
            List<FoodItem> foodItems = categoryId == 0
            ? _foodItemService.GetAllFoodItems()
            : _foodItemService.GetFoodItemsByCategory(categoryId);
            ViewBag.ShowEditDelete = true;
            return PartialView("_FoodItemsPartial", foodItems);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new FoodItemViewModel
            {
                Categories = _foodItemService.GetAllCategories()
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(FoodItemViewModel viewModel, IFormFile ImageFile)
        {
            
            var foodItem = new FoodItem
                {
                    FoodName = viewModel.FoodName,
                    FoodDescription = viewModel.FoodDescription,
                    FoodPrice = viewModel.FoodPrice,
                    CategoryId = viewModel.CategoryId,
                    ImageUrl = viewModel.ImageUrl,
                   
            };
            
                _foodItemService.AddFoodItem(foodItem,ImageFile);

                return RedirectToAction("Index");
            

           
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var foodItem = _foodItemService.GetFoodItemById(id);
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
                Categories = _foodItemService.GetAllCategories(),
                ImageUrl = foodItem.ImageUrl
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(FoodItemViewModel viewModel, IFormFile ImageFile)
        {
                var foodItem = _foodItemService.GetFoodItemById(viewModel.Id);
                if (foodItem == null)
                {
                    return NotFound();
                }

                // Handle file upload if a new image is provided
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // Generate unique file name
                    string fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(ImageFile.FileName)}";
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Files", fileName);

                    // Copy file to server
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    // Delete old file if it exists
                    if (!string.IsNullOrEmpty(foodItem.ImageUrl))
                    {
                        string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "Files", foodItem.ImageUrl);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    // Update food item with new image URL
                    foodItem.ImageUrl = fileName;
                }

                // Update other properties
                foodItem.FoodName = viewModel.FoodName;
                foodItem.FoodDescription = viewModel.FoodDescription;
                foodItem.FoodPrice = viewModel.FoodPrice;
                foodItem.CategoryId = viewModel.CategoryId;

                _foodItemService.UpdateFoodItem(foodItem);
                return RedirectToAction("Index");
            

        
            viewModel.Categories = _foodItemService.GetAllCategories();
            return View(viewModel);
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            var foodItem = _foodItemService.GetFoodItemById(id);
            if (foodItem == null)
            {
                return NotFound();
            }

            return View(foodItem);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var foodItem = _foodItemService.GetFoodItemById(id);
            if (foodItem == null)
            {
                return NotFound();
            }

            _foodItemService.DeleteFoodItem(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var foodItem = _foodItemService.GetFoodItemById(id);
            if (foodItem == null)
            {
                return NotFound();
            }

            return View(foodItem);
        }
    }
}

