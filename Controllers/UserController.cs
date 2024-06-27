using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Restaurant_WebApp.Data;
using Restaurant_WebApp.Models;
using Restaurant_WebApp.Models.ViewModels;
using Restaurant_WebApp.Repos.Interface;
using Restaurant_WebApp.Repos.Services;


namespace Restaurant_WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IUserServices _userServices;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IOrderServices _orderServices;

        public UserController(ApplicationDbContext db, IUserServices userServices,
            UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
            IOrderServices orderServices)
        {
            _db = db;
            _userServices = userServices;
            _roleManager = roleManager;
            _userManager = userManager;
            _orderServices = orderServices;
        }


        public async Task<IActionResult> Index()
        {
            var userWithRoles = await _userServices.GetAllUserAsync();
            return View(userWithRoles);
        }


        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("User ID is required");
            }

            var userWithRoles = await _userServices.GetUserByIdAsync(id);

            if (userWithRoles == null || userWithRoles.Roles.Count == 0)
            {
                ViewBag.NoRole = "No Roles Assigned";
            }
            //userWithRoles.AllRoles = _roleManager.Roles
            //    .Select(r => new SelectListItem
            //    {
            //        Value = r.Name,
            //        Text = r.Name
            //    }).ToList();

            return View(userWithRoles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserWithRolesVM model)
        {
            var result = _userServices.UpdateUserAsync(model);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userServices.DeleteUserAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(string id)
        {
            var detail = await _userServices.GetUserByIdAsync(id);
            if (detail == null)
            {
                return NotFound();
            }
            return View(detail);

        }

        public async Task<IActionResult> RoleIndex()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }


        [HttpGet]
        public async Task<IActionResult> CreateRole()
        {


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(string roleName)
        {

            var result = await _userServices.CreateRoleAsync(roleName);

            if (result)
            {

                return RedirectToAction("RoleIndex");
            }
            else
            {

                return View("Error");
            }

        }

        [HttpGet]
        public async Task<IActionResult> AssignRole()
        {
            var users = await _userManager.Users.ToListAsync();
            var roles = await _roleManager.Roles.ToListAsync();

            var model = new UserWithRolesVM
            {
                Users = users,
                AllRoles = roles.Select(r => new SelectListItem
                {
                    Value = r.Id,
                    Text = r.Name
                }).ToList()
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AssignRole(UserWithRolesVM model)
        {

            await _userServices.AssignRoleToUserAsync(model.SelectedUserId, model.SelectedRoleId);

            // Re-populate the lists in case of error
            model.Users = await _userManager.Users.ToListAsync();
            var roles = await _roleManager.Roles.ToListAsync();
            model.AllRoles = roles.Select(r => new SelectListItem
            {
                Value = r.Id,
                Text = r.Name
            }).ToList();

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> RemoveRole()

        {
            var users = await _userManager.Users.ToListAsync();
            var roles = await _roleManager.Roles.ToListAsync();

            var model = new UserWithRolesVM
            {
                Users = users,
                AllRoles = roles.Select(r => new SelectListItem
                {
                    Value = r.Id,
                    Text = r.Name
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(UserWithRolesVM model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.SelectedUserId) && !string.IsNullOrEmpty(model.SelectedRoleId))
                {
                    await _userServices.RemoveRoleFromUserAsync(model.SelectedUserId, model.SelectedRoleId);
                    TempData["SuccessMessage"] = "Role removed successfully from user.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Please select both a user and a role.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to remove role from user: {ex.Message}";
            }

            return RedirectToAction("RemoveRole");
        }


        public async Task<IActionResult> OrderList()
        {
            var order = await _userServices.CustomerOrdersAsync();
            return View(order);
        }


    }

}
