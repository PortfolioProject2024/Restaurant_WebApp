using Microsoft.AspNetCore.Identity;

namespace Restaurant_WebApp.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public ICollection<Customer>? Customers { get; set; }
    }
}
