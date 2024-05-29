using Microsoft.AspNetCore.Mvc;
using Restaurant_WebApp.Data;
using Restaurant_WebApp.Models;
using Restaurant_WebApp.Repos.Interface;

namespace Restaurant_WebApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerServices _customerServices;
        private readonly ApplicationDbContext _db;

        public CustomerController(ICustomerServices customerServices, ApplicationDbContext db)
        {
            _db = db;
            _customerServices = customerServices;
        }

        public async Task<IActionResult> Index()
        {
            var custList = await _customerServices.GetAllCustomersAsync();
            return View(custList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Customer newCustomer, int orderId)
        {
            var addCustomer = await _customerServices.AddCustomerAsync(newCustomer, orderId);

            return View(addCustomer);
        }

        public async Task<IActionResult> Edit()
        {
            return View();
        }
    }
}
