using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_WebApp.Data;
using Restaurant_WebApp.Models;
using Restaurant_WebApp.Repos.Interface;
using System.Security.Claims;

namespace Restaurant_WebApp.Controllers
{
    public class TableController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ITableBookingServices _tableBookingServices;
        private readonly UserManager<User> _userManager;


        public TableController(ApplicationDbContext db, ITableBookingServices tableBookingServices,
            UserManager<User> userManager)
        {
            _db = db;
            _tableBookingServices = tableBookingServices;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(int customerId, string userId)
        {
            var bookingList = await _tableBookingServices.GetCustomerBookingsAsync(customerId, userId);
            return View(bookingList);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TableBooking tableBooking, int customerId)
        {
           
            var booking = await _tableBookingServices.AddTableBookingAsync(tableBooking, customerId);
            return RedirectToAction("Index");
        }


       
    }
}
