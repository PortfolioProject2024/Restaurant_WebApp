using System.ComponentModel.DataAnnotations;

namespace Restaurant_WebApp.Models
{
    public class TableBooking
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? CustomerName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [Phone]
        public string? PhoneNumber { get; set; }
        [Required]
        [Range(1, 20, ErrorMessage = "Total persons must be between 1 and 20.")]
        public int TotalPersons { get; set; }
        [Required]
        public DateTime BookingDate { get; set; }

        public TimeSpan? StartingTime { get; set; }
        public TimeSpan? EndingTime { get; set; }

        public string? Comments { get; set; }

        // Virtual Props
        public int? CustomerId { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }

        public virtual ICollection<Customer>? Customer { get; set; }

    }
}
