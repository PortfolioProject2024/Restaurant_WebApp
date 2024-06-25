using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_WebApp.Models;
using Restaurant_WebApp.Repos.Interface;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant_WebApp.Controllers
{
    public class UserOrderController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IOrderServices _orderServices;
        private readonly IFoodItemService _foodItemServices;

        public UserOrderController(IOrderServices orderServices, IFoodItemService foodItemServices, UserManager<User> userManager)
        {
            _orderServices = orderServices;
            _foodItemServices = foodItemServices;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var order = await _orderServices.GetOrCreateActiveOrderAsync(user.Id);
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
                return RedirectToAction("Login", "Account");
            }

            var order = await _orderServices.GetOrCreateActiveOrderAsync(user.Id);
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

            await _orderServices.UpdateOrderAsync(order);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderServices.GetOrderByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            await _orderServices.IncludeOrderItemsAsync(order);
            await _foodItemServices.IncludeFoodItemsAsync(order);

            return View(order);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderServices.GetOrderByIdAsync(id);
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
                    await _orderServices.UpdateOrderAsync(order);
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
            var order = await _orderServices.GetOrderByIdAsync(id);
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
            await _orderServices.DeleteOrderAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> OrderExists(int id)
        {
            var order = await _orderServices.GetOrderByIdAsync(id);
            return order != null;
        }
    }
}
