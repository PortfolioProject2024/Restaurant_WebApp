using Microsoft.AspNetCore.Mvc;
using Restaurant_WebApp.Migrations;
using Restaurant_WebApp.Models;
using Restaurant_WebApp.Repos.Interface;
using System.Diagnostics;
using Restaurant_WebApp.ViewModels;

namespace Restaurant_WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFoodItemService _foodItemService;
        public HomeController(ILogger<HomeController> logger,IFoodItemService foodItemService )
        {
            _foodItemService = foodItemService;
            _logger = logger;
            
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

        public IActionResult CoreAdmin()

        {

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
