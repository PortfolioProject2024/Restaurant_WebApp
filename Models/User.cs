using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_WebApp.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
   
        public int? RewardPoints { get; set; }
        public int? DiscountPercentage { get; set; }

        public DateTime? DOB { get; set; }

        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }

        public virtual ICollection<Order>? Orders { get; set; } = new List<Order>();

        public virtual ICollection<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();

        public virtual ICollection<TableBooking>? TableBookings { get; set; }

        public virtual ICollection<Feedback>? Feedbacks { get; set; }
    }
}
