using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Restaurant_WebApp.Data;
using Restaurant_WebApp.Models;
using Restaurant_WebApp.Repos.Interface;
using Stripe.Checkout;
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
        private readonly StripeSettings _stripeSettings;

        public UserOrderController(IOrderItemServices orderItemServices, IFoodItemServices foodItemServices, UserManager<User> userManager, ApplicationDbContext db, IHttpContextAccessor httpContextAccessor, IOptions<StripeSettings> stripeSettings)
        {

            _foodItemServices = foodItemServices;
            _userManager = userManager;
            _orderItemServices = orderItemServices;
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _stripeSettings = stripeSettings.Value;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }


            var order = await _orderItemServices.GetOrCreateActiveOrderAsync(user.Id);
            await _orderItemServices.IncludeOrderItemsAsync(order);
            await _foodItemServices.IncludeFoodItemsAsync(order);

            return View(order);
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
                return RedirectToAction("Login", "Account", new { area = "Identity" });
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromOrder(int orderItemId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
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
                return Json(new { success = false, message = "User not logged in" });
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
                    return Json(new { success = false, message = "Food item not found" });
                }

                var newOrderItem = new OrderItem
                {
                    FoodItemId = foodItemId,
                    Quantity = quantity
                };

                order.OrderItems.Add(newOrderItem);
            }

            await _orderItemServices.UpdateOrderAsync(order);
            var cartItemCount = order.OrderItems.Sum(oi => oi.Quantity);

            return Json(new { success = true, cartItemCount });
        }



        public async Task<IActionResult> GetCartItemCount()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "User not logged in" });
            }

            var order = await _orderItemServices.GetOrCreateActiveOrderAsync(user.Id);
            var cartItemCount = order.OrderItems.Sum(oi => oi.Quantity);

            return Json(new { success = true, cartItemCount });
        }

        public async Task<IActionResult> Checkout()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var order = await _orderItemServices.GetOrCreateActiveOrderAsync(user.Id);
            await _orderItemServices.IncludeOrderItemsAsync(order);
            await _foodItemServices.IncludeFoodItemsAsync(order);

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = order.OrderItems.Select(item => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.FoodItems.FoodPrice * 100), // pris i ören
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.FoodItems.FoodName,
                        },
                    },
                    Quantity = item.Quantity,
                }).ToList(),
                Mode = "payment",
                SuccessUrl = Url.Action("Success", "UserOrder", null, Request.Scheme),
                CancelUrl = Url.Action("Cancel", "UserOrder", null, Request.Scheme),
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return Redirect(session.Url);
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Cancel()
        {
            return View();
        }
    }
}