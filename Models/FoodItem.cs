using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant_WebApp.Models
{
    public class FoodItem
    {
        [Key]
        public int? Id { get; set; }
        [Required]
        [StringLength(100)]
        public string? FoodName { get; set; }
        public string? FoodDescription { get; set; }

        // Virtual Props
        public decimal? FoodPrice { get; set; }
   
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();

       
    }
}
