using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_WebApp.Data;
using Restaurant_WebApp.Models;
using Restaurant_WebApp.Repos.Interface;

namespace Restaurant_WebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ICartServices _cartServices;

        public CartController(ICartServices cartServices, ApplicationDbContext db)
        {
            _cartServices = cartServices;
            _db = db;
        }

       
        public IActionResult Cart()
        {
            List<CartItem> cartItems = _cartServices.GetCartItems();
            return View(cartItems);
        }

      
        public IActionResult AddToCart(int foodItemId, int quantity)
        {
            FoodItem foodItemToAdd = GetFoodItemFromDatabase(foodItemId); // Implementera denna metod för att hämta matobjekt från databasen
            _cartServices.AddToCart(foodItemToAdd, quantity);
            return RedirectToAction("Cart");
        }

        // Exempel på en åtgärd för att ta bort matobjekt från kundvagnen
        public IActionResult RemoveFromCart(int foodItemId)
        {
            _cartServices.RemoveFromCart(foodItemId);
            return RedirectToAction("Cart");
        }

        // Exempel på en åtgärd för att rensa hela kundvagnen
        public IActionResult ClearCart()
        {
            _cartServices.ClearCart();
            return RedirectToAction("Cart");
        }
        private FoodItem GetFoodItemFromDatabase(int foodItemId)
        {
            return _db.FoodItems.FirstOrDefault(fi => fi.Id == foodItemId);
        }
    }
}
