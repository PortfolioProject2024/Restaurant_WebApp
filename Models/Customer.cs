using System.ComponentModel.DataAnnotations;

namespace Restaurant_WebApp.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [Phone]
        public string? PhoneNumber { get; set; }
        public string? UserId { get; set; }

        // Virtual Props
        public int? RewardPoints { get; set; }
        public int? DiscountPercentage { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<Order>? Orders { get; set; } = new List<Order>();

        public ICollection<TableBooking>? Bookings { get; set; } 
    }
}
