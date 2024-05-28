using System.ComponentModel.DataAnnotations;

namespace Restaurant_WebApp.Models
{
    public class FoodItem
    {
        [Key]
        public int? Id { get; set; }
        [Required]
        [StringLength(100)]
        public string FoodName { get; set; }
        public string? FoodDescription { get; set; }

        // Virtual Props
        public decimal? FoodPrice { get; set; }
        public string? IMageUrl { get; set; }
        public virtual ICollection<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();
    }
}
