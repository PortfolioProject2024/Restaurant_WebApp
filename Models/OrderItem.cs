﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_WebApp.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }

        public int? Quantity { get; set; }

        public virtual Order? Order { get; set; }
        [ForeignKey("FoodItem")]
        public int FoodItemId { get; set; }
        public virtual FoodItem? FoodItems { get; set; }
    }
}
