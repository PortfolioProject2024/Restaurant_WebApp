using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_WebApp.Data;
using Restaurant_WebApp.Models;
using Restaurant_WebApp.Repos.Interface;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant_WebApp.Controllers
{
    public class UserOrderController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IOrderItemServices _orderItemServices;
        private readonly IFoodItemServices _foodItemServices;
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserOrderController(IOrderItemServices orderItemServices, IFoodItemServices foodItemServices, UserManager<User> userManager, ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {

            _foodItemServices = foodItemServices;
            _userManager = userManager;
            _orderItemServices = orderItemServices;
            _db = db;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }


            var order = await _orderItemServices.GetOrCreateActiveOrderAsync(user.Id);
            await _orderItemServices.IncludeOrderItemsAsync(order);
            await _foodItemServices.IncludeFoodItemsAsync(order);


            return View(order);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int foodItemId, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToPage("/Account/Register", new { area = "Identity", foodItemId, quantity });
            }

            // Handle the scenario where TempData was used to pass parameters
            if (TempData["FoodItemId"] != null && TempData["Quantity"] != null)
            {
                foodItemId = (int)TempData["FoodItemId"];
                quantity = (int)TempData["Quantity"];
            }

            var order = await _orderItemServices.GetOrCreateActiveOrderAsync(user.Id);
            var existingOrderItem = order.OrderItems.FirstOrDefault(oi => oi.FoodItemId == foodItemId);

            if (existingOrderItem != null)
            {
                existingOrderItem.Quantity += quantity;
            }
            else
            {
                var newOrderItem = new OrderItem
                {
                    FoodItemId = foodItemId,
                    Quantity = quantity
                };

                order.OrderItems.Add(newOrderItem);
            }

            await _orderItemServices.UpdateOrderAsync(order);

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderItemServices.GetOrderByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }


            await _orderItemServices.IncludeOrderItemsAsync(order);
            await _foodItemServices.IncludeFoodItemsAsync(order);


            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantity(int orderItemId, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var order = await _orderItemServices.GetOrCreateActiveOrderAsync(user.Id);
            var orderItem = order.OrderItems.FirstOrDefault(oi => oi.Id == orderItemId);

            if (orderItem != null)
            {
                orderItem.Quantity = quantity;
                await _orderItemServices.UpdateOrderAsync(order);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromOrder(int orderItemId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var order = await _orderItemServices.GetOrCreateActiveOrderAsync(user.Id);
            var orderItem = order.OrderItems.FirstOrDefault(oi => oi.Id == orderItemId);

            if (orderItem != null)
            {
                order.OrderItems.Remove(orderItem);
                await _orderItemServices.UpdateOrderAsync(order);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int foodItemId, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var order = await _orderItemServices.GetOrCreateActiveOrderAsync(user.Id);
            var existingOrderItem = order.OrderItems.FirstOrDefault(oi => oi.FoodItemId == foodItemId);

            if (existingOrderItem != null)
            {
                
                existingOrderItem.Quantity += quantity;
            }
            else
            {
                
                var foodItem = await _foodItemServices.GetFoodItemByIdAsync(foodItemId);
                if (foodItem == null)
                {
                    return NotFound(); 
                }

              
                var newOrderItem = new OrderItem
                {
                    FoodItemId = foodItemId,
                    Quantity = quantity
                   
                };

                
                order.OrderItems.Add(newOrderItem);
            }

            
            await _orderItemServices.UpdateOrderAsync(order);

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Checkout()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var order = await _orderItemServices.GetOrCreateActiveOrderAsync(user.Id); 
            
            order.CompletedTimestamp = DateTime.Now; 

            await _orderItemServices.UpdateOrderAsync(order);
            order.IsCompleted = false;
            
            return RedirectToAction("Index", "Home");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClearOrder()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var order = await _orderItemServices.GetOrCreateActiveOrderAsync(user.Id);
            order.OrderItems.Clear();
            await _orderItemServices.UpdateOrderAsync(order);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderItemServices.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _orderItemServices.UpdateOrderAsync(order);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderItemServices.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _orderItemServices.DeleteOrderAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> OrderExists(int id)
        {
            var order = await _orderItemServices.GetOrderByIdAsync(id);
            return order != null;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateFoodItemComment(int orderItemId, string foodItemComment)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var order = await _orderItemServices.GetOrCreateActiveOrderAsync(user.Id);
            var orderItem = order.OrderItems.FirstOrDefault(oi => oi.Id == orderItemId);

            if (orderItem != null)
            {
                orderItem.Comment = foodItemComment; 
                await _orderItemServices.UpdateOrderAsync(order);
            }

            return RedirectToAction(nameof(Index), new { id = order.Id }); 
        }


    }
}