using Microsoft.AspNetCore.Mvc;

namespace Restaurant_WebApp.Controllers
{
    public class UserController : Controller
    {


        public IActionResult Index()
        {
            return View();
        }
    }
}
