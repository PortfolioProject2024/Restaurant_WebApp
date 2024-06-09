using Microsoft.AspNetCore.Mvc;
using Restaurant_WebApp.Data;
using Restaurant_WebApp.Repos.Interface;

namespace Restaurant_WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IUserServices _userServices;
        public UserController(ApplicationDbContext db, IUserServices userServices)
        {
            _db = db;   
            _userServices = userServices;
        }

        public async Task<IActionResult> Index()
        {
            var userList = _userServices.GetAllUserAsync();
            return View(userList);
        }
    }
}
