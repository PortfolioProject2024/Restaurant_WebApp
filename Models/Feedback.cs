using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_WebApp.Models
{
    public class Feedback
    {
        [Key]
        public int? Id { get; set; }

        [Range(1, 5)]
        public int? Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime? Date { get; set; }

        // Virtual Props
        [ForeignKey("FoodItem")]
        public int? FoodItemId { get; set; }
        public virtual FoodItem? FoodItem { get; set; }

        [ForeignKey("Customer")]
        public int? CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
    }
}
