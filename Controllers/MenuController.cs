using Microsoft.AspNetCore.Mvc;
using Restaurant_WebApp.Models;
using Restaurant_WebApp.Repos.Interface;
using Restaurant_WebApp.ViewModels;

namespace Restaurant_WebApp.Controllers
{
    public class MenuController : Controller
    {
        private readonly IFoodItemService _foodItemService;

        public MenuController(IFoodItemService foodItemService)
        {
            _foodItemService = foodItemService;
        }

        public IActionResult Index()
        {
            var viewModel = new MenuViewModel
            {
                FoodItems = _foodItemService.GetAllFoodItems(),
                Categories = _foodItemService.GetAllCategories()
            };
             ViewData["MenuViewModel"] = viewModel;
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult FilterByCategory(int categoryId)
        {
            List<FoodItem> foodItems = _foodItemService.GetFoodItemsByCategory(categoryId);
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
                   
            };
            
                _foodItemService.AddFoodItem(foodItem,ImageFile);

                return RedirectToAction("Index");
            

           
        }

    }
}
