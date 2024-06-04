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
        public async Task<IActionResult> Index(int tableId)
        {
            var bookingList = await _tableBookingServices.GetAllBookingsAsync(tableId);
            return View(bookingList);
        }

        //public async Task<IActionResult> Create() 
        //{
        //    return View();
        //}

        [HttpPost]
        public async Task<IActionResult> Create(TableBooking tableBooking)
        {
            var addBooking = _tableBookingServices.AddTableBookingAsync(tableBooking);
            return View(addBooking);
        }

       

        public async Task<IActionResult> Delete(int Id) 
        {
            await _tableBookingServices.DeleteTableBookingAsync(Id);
            return RedirectToAction("Index");
        }

    }
}
