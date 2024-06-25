using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_WebApp.Models
{
    public class OrderItem
    {
      
        public int Id { get; set; }

        public int Quantity { get; set; } = 1;

        // Virtual Props
        [ForeignKey("Order")]
        public int OrderId { get; set; }
     
        public virtual Order? Order { get; set; }
        [ForeignKey("FoodItem")]
        public int? FoodItemId { get; set; }
        public virtual FoodItem? FoodItems { get; set; }
        public string? Comment { get; set; }
        
    }
}
