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
            if (ModelState.IsValid)
            {
                await _tableBookingServices.AddTableBookingAsync(tableBooking);

                // Redirect to the Success view with query parameters
                return RedirectToAction("Success", 
                    new { name = tableBooking.CustomerName, phone = tableBooking.PhoneNumber });
            }
            else
            {
                return View(tableBooking);
            }
        }

        public async Task<IActionResult> Success(string phone, string name)
        {
            var message = await _tableBookingServices.ConfirmationMessageAsync(phone, name);

            return View(message);
        }



        public async Task<IActionResult> Delete(int Id)
        {
            await _tableBookingServices.DeleteTableBookingAsync(Id);
            return RedirectToAction("Index");
        }
        
     

        [HttpGet]
        public async Task<IActionResult> Edit(int id) 
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _tableBookingServices.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TableBooking tableBooking)
        {
            await _tableBookingServices.UpdateBookingAsync(tableBooking);
            return RedirectToAction("Index");
        }

    }
}
