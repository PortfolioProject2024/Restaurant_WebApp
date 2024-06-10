using Restaurant_WebApp.Models;
using System.Collections.Generic;

namespace Restaurant_WebApp.ViewModels
{
    public class MenuViewModel
    {
        public List<FoodItem> FoodItems { get; set; }
        public List<Category> Categories { get; set; }
    }
}
