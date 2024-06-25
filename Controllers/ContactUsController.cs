using Microsoft.AspNetCore.Mvc;
using Restaurant_WebApp.Data;
using Restaurant_WebApp.Migrations;
using Restaurant_WebApp.Models;
using Restaurant_WebApp.Repos.Interface;
using Restaurant_WebApp.Repos.Services;

namespace Restaurant_WebApp.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly IContactUsService _contactService;
        private readonly ApplicationDbContext _db;

        public ContactUsController(IContactUsService contactService, ApplicationDbContext db)
        {
            _contactService = contactService;
            _db = db;
        }


        public async Task<IActionResult> Index()
        {

            var messages = await _contactService.GetAllMessagesAsync();
            return View(messages);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ContactUs contact)
        {
            await _contactService.AddMessageAsync(contact);
            ViewData["Message"] = "Your message has been sent successfully";
            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> Delete(int id) 
        {
            await _contactService.DeleteMessageAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var messageDetail = await _contactService.GetMessageByIdAsync(id);
            if (messageDetail == null)
            {
                return NotFound();
            }
            return View(messageDetail);
        }
    }
}
