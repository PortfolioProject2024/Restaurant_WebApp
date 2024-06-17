﻿namespace Restaurant_WebApp.Models.ViewModels
{
    public class UserWithRolesVM
    {
        public string? Id { get; set; }
        public string? FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public List<string> Roles { get; set; }

        public ICollection<Customer> Customers { get; set; }
    }
}