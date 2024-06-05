using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant_WebApp.Models;
using Restaurant_WebApp.Repos.Interface;

namespace Restaurant_WebApp.Controllers
{
    public class MenuController : Controller
    {
        private readonly IFoodItemService _foodItemService;

        public MenuController(IFoodItemService foodItemService)
        {
            _foodItemService = foodItemService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var foodItems = await _foodItemService.GetFoodItems();
                if (foodItems == null)
                {
                    
                    foodItems = new List<FoodItem>();
                }
                return View(foodItems);
            }
            catch (Exception ex)
            {
                
                return View(new List<FoodItem>());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var foodItem = await _foodItemService.GetFoodItem(id);
            if (foodItem == null)
            {
                return NotFound();
            }
            return View(foodItem);
        }

        public  IActionResult Create()
        {
           
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FoodItem foodItem)
        {
            if (ModelState.IsValid)
            {
                await _foodItemService.CreateFoodItem(foodItem);
                return RedirectToAction(nameof(Index));
            }
            return View(foodItem);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var foodItem = await _foodItemService.GetFoodItem(id);
            if (foodItem == null)
            {
                return NotFound();
            }
            return View(foodItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FoodItem foodItem)
        {
            if (id != foodItem.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _foodItemService.UpdateFoodItem(foodItem);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    return View("Error");
                }
            }
            return View(foodItem);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var foodItem = await _foodItemService.GetFoodItem(id);
            if (foodItem == null)
            {
                return NotFound();
            }
            return View(foodItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _foodItemService.DeleteFoodItem(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
    }

}
