using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_WebApp.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }


        public DateTime? OrderDate { get; set; } = DateTime.Now;

        public decimal? TotalPrice { get; set; }

        public string? SpecialComment { get; set; }

        // Virtual Props
        public string? UserId { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();
        public bool IsCompleted { get; set; } 
    }
}
