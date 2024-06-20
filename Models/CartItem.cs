using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_WebApp.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("FoodItem")]
        public int FoodItemId { get; set; }
        public virtual FoodItem FoodItem { get; set; }

        public int Quantity { get; set; }
        public string CartId { get; set; } = string.Empty;
    }
}
