using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_WebApp.Models
{
    public class Order
    {
        [Key]
        public int? Id { get; set; }


        public DateTime? OrderDate { get; set; }

        public decimal? TotalPrice { get; set; }

        // Virtual Props

        [ForeignKey("Customer")]
        public int? CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual ICollection<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();
    }
}
