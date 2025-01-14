﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_WebApp.Models.ViewModels
{
    public class UserWithRolesVM
    {
        public UserWithRolesVM()
        {
            AllRoles = new List<SelectListItem>();
        }
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public List<string>? Roles { get; set; }
        public string UserId { get; set; }
        public List<User> Users { get; set; } 
   
        public string SelectedUserId { get; set; }
        public string SelectedRoleId { get; set; }
        public bool IsSelected { get; set; }
        [Display(Name = "Role Name")]
        public string? RoleName { get; set; }
        //public ICollection<Customer>? Customers { get; set; }
        public List<SelectListItem>? AllRoles { get; set; }

       

        public virtual ICollection<Order>? Orders { get; set; } = new List<Order>();

        public virtual ICollection<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();


    }
}
