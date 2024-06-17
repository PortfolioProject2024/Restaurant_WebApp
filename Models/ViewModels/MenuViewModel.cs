using Restaurant_WebApp.Models.ViewModels;
using System.Collections.Generic;

namespace Restaurant_WebApp.Models.ViewModels
{   
    public class MenuViewModel
    {
        public List<FoodItem>? FoodItems { get; set; }
        public List<Category>? Categories { get; set; }

    }
}
