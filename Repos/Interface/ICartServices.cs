using Restaurant_WebApp.Models;
using System.Collections.Generic;

namespace Restaurant_WebApp.Repos.Interface
{
    public interface ICartServices
    {
        List<CartItem> GetCartItems();
        void AddToCart(FoodItem foodItem, int quantity);
        void RemoveFromCart(int foodItemId);
        void ClearCart();
    }
}