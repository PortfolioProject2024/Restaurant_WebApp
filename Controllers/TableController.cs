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
                return RedirectToAction("Success");
            }
            else
            {
                return View(tableBooking);
            }
        }

        public async Task<IActionResult> Success(TableBooking tableBooking)
        {
            string confirmMessage = await _tableBookingServices.ConfirmationMessage(tableBooking);

            if (string.IsNullOrEmpty(confirmMessage))
            {
                ViewBag.ErrorMessage = "An error occurred while processing your booking. Please try again later.";
            }
            else
            {
                ViewBag.ErrorMessage = confirmMessage;
            }
            return View();
        }



        public async Task<IActionResult> Delete(int Id)
        {
            await _tableBookingServices.DeleteTableBookingAsync(Id);
            return RedirectToAction("Index");
        }



    }
}
