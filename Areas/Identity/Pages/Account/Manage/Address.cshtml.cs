using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using Restaurant_WebApp.Data;
using Restaurant_WebApp.Models;
using Restaurant_WebApp.Repos.Interface;

namespace Restaurant_WebApp.Areas.Identity.Pages.Account.Manage
{
    public class AddressViewModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly IUserServices _userServices;


        public AddressViewModel(UserManager<User> userManager, ApplicationDbContext db,
            IUserServices userServices)
        {
            _userManager = userManager;
            _db = db;
            _userServices = userServices;

        }

        public User CurrentUser { get; set; }

        [BindProperty]
        public string Address { get; set; }
        [BindProperty]
        public string City { get; set; }
        [BindProperty]
        public string Country { get; set; }
        [BindProperty]
        public string PostalCode { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            if (CurrentUser == null)
            {
                return NotFound("User not found");
            }

            // Populate the properties with current address details
            Address = CurrentUser.Address;
            City = CurrentUser.City;
            Country = CurrentUser.Country;
            PostalCode = CurrentUser.PostalCode;

            return Page();

        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Update user's address
            user.Address = Address;
            user.City = City;
            user.Country = Country;
            user.PostalCode = PostalCode;

            _db.Users.Update(user);
            await _db.SaveChangesAsync();


            TempData["SuccessMessage"] =  $"Dear {user.FirstName}! Your address has been successfully updated.";
            

            return Page();
        }


    }

}