using Restaurant_WebApp.Models;
using Restaurant_WebApp.Repos.Interface;
using System.Collections.Generic;
using System.Linq;

public class CartServices : ICartServices
{
    private readonly List<CartItem> _cartItems;

    public CartServices()
    {
        // Initialisera kundvagnslistan
        _cartItems = new List<CartItem>();
    }

    public List<CartItem> GetCartItems()
    {
        return _cartItems;
    }

    public void AddToCart(FoodItem foodItem, int quantity)
    {
        // Kolla om matobjektet redan finns i kundvagnen, om det gör det, öka antalet, annars lägg till nytt
        CartItem existingCartItem = _cartItems.FirstOrDefault(ci => ci.FoodItemId == foodItem.Id);
        if (existingCartItem != null)
        {
            existingCartItem.Quantity += quantity;
        }
        else
        {
            _cartItems.Add(new CartItem
            {
                FoodItemId = foodItem.Id,
                FoodItem = foodItem,
                Quantity = quantity
            });
        }
    }

    public void RemoveFromCart(int foodItemId)
    {
        CartItem cartItemToRemove = _cartItems.FirstOrDefault(ci => ci.FoodItemId == foodItemId);
        if (cartItemToRemove != null)
        {
            _cartItems.Remove(cartItemToRemove);
        }
    }

    public void ClearCart()
    {
        _cartItems.Clear();
    }
}
