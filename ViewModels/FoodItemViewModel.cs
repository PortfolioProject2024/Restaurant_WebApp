using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Restaurant_WebApp.Models;
using Microsoft.AspNetCore.Http;

namespace Restaurant_WebApp.ViewModels
{
    public class FoodItemViewModel

    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string FoodName { get; set; }

        public string FoodDescription { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal? FoodPrice { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public List<Category> Categories { get; set; }

        public IFormFile ImageFile { get; set; }
    }
}
