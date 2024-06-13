using Microsoft.AspNetCore.Mvc.Rendering;

namespace Restaurant_WebApp.Models.ViewModels
{
    public class UserWithRolesVM
    {
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public List<string>? Roles { get; set; }
        public string? SelectedRole { get; set; }

        public List<SelectListItem>? AllRoles { get; set; }
       
        public ICollection<Customer>? Customers { get; set; }
    }
}
