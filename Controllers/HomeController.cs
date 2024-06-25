using Microsoft.AspNetCore.Mvc;
using Restaurant_WebApp.Models;
using Restaurant_WebApp.Repos.Interface;
using System.Diagnostics;
using Restaurant_WebApp.Models.ViewModels;

namespace Restaurant_WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFoodItemService _foodItemService;
        private readonly IContactUsService _contactService;
        public HomeController(ILogger<HomeController> logger, IFoodItemService foodItemService,
            IContactUsService contactService)
        {
            _foodItemService = foodItemService;
            _logger = logger;
            _contactService = contactService;

        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new MenuViewModel
            {
                FoodItems = await _foodItemService.GetAllFoodItemsAsync(),
                Categories = await _foodItemService.GetAllCategoriesAsync()
            };

            ViewBag.Form = new ContactUs();

            int messageCount = await _contactService.GetMessagesCountAsync();
            ViewBag.MessageCount = messageCount;

            ViewData["MenuViewModel"] = viewModel;
            ViewBag.ShowEditDelete = false;
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
