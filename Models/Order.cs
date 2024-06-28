using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_WebApp.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }


        public DateTime? OrderDate { get; set; } = DateTime.Now;

        public decimal? TotalPrice
        {
            get
            {
                return OrderItems?.Sum(item => item.FoodItems.FoodPrice * item.Quantity) ?? 0;
            }
        }
        public string? SpecialComment { get; set; }
        public bool IsCompleted { get; set; } 
        public DateTime? CompletedTimestamp { get; set; }
     
        public string? UserId { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();
       
    }
}
