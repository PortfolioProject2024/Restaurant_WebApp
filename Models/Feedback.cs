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

        public string? Name { get; set; }
        public string? Designation { get; set; }

        public DateTime? Date { get; set; }

        // Virtual Props
        [ForeignKey("FoodItem")]
        public int? FoodItemId { get; set; }
        public virtual FoodItem? FoodItem { get; set; }

        [ForeignKey("User")]
        public int? UserID { get; set; }
        public virtual ICollection<User>? User { get; set; }
    }
}
