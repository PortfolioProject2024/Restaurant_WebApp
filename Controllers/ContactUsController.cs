﻿using Microsoft.AspNetCore.Mvc;
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
            var list = await _contactService.GetAllMessagesAsync();
            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ContactUs contact)
        {
            await _contactService.AddMessageAsync(contact);
            ViewData["Message"] = "Your message has been sent successfully";
            return RedirectToAction("Index", "Home");
        }

    }
}