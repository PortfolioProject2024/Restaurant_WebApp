using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
        private readonly IEmailSender _emailSender;


        public TableController(ApplicationDbContext db, ITableBookingServices tableBookingServices,
            UserManager<User> userManager, IEmailSender emailSender)
        {
            _db = db;
            _tableBookingServices = tableBookingServices;
            _userManager = userManager;
            _emailSender = emailSender;
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
                // Ensure the time component is set to 00:00:00
                tableBooking.BookingDate = tableBooking.BookingDate.Date;

                // Add the table booking asynchronously
                await _tableBookingServices.AddTableBookingAsync(tableBooking);

                // Get the user's email from the booking details
                var userEmail = tableBooking.Email;

                var htmlEmailBody = $"<p>Dear {tableBooking.CustomerName},</p>" +
                               $"<p>Your booking is confirmed with booking <b>ID</b> {tableBooking.Id} on " +
                               $"{tableBooking.BookingDate:MMMM dd, yyyy} <b>From</b> {tableBooking.StartingTime} <b>To</b>" +
                               $"{tableBooking.EndingTime}.</p>" +
                               $"<p>Warm Welcome,</p>" +
                               $"<p>The Flavor Loft</p>";

                await _emailSender.SendEmailAsync(userEmail, "Booking Confirmation", htmlEmailBody);

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
        
        public IActionResult Update (int Id ) {

            var updateBooking = _db.FindById(Id);

            if (updateBooking == null)
            {
                return NotFound();
            }
            var tableBooking = new TableBooking();
            tableBooking.CustomerName = "New Customer Name";
            tableBooking.BookingDate = DateTime.Now;
            _db.SaveChanges();

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
