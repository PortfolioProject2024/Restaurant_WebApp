using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Restaurant_WebApp.Data;
using Restaurant_WebApp.Repos.Interface;
using Restaurant_WebApp.Repos.Services;

namespace Restaurant_WebApp.Controllers
{
    public class ContactController
    {
        private readonly IContactService _contactService;
        private readonly ApplicationDbContext _db;

        public ContactController(IContactService contactService, ApplicationDbContext db)
        {
            _db = db;
            _contactService = contactService;
        }




                





    }
}
