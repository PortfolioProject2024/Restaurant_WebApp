using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurant_WebApp.Data;

namespace Restaurant_WebApp.Models.SeedData
{
    public class SeedData
    {
        public async static Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await SeedRoles(roleManager);
            await SeedAdmin(userManager);
            


        }


        private async static Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("superadmin"))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = "superadmin" });
            }
            if (!await roleManager.RoleExistsAsync("admin"))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = "admin" });
            }
            if (!await roleManager.RoleExistsAsync("employee"))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = "employee" });
            }
            if (!await roleManager.RoleExistsAsync("customer"))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = "customer" });
            }
        }

        private async static Task SeedAdmin(UserManager<User> userManager)
        {
            var superadmin = await userManager.FindByEmailAsync("admin@mail.com");
            var useradmin = await userManager.FindByEmailAsync("portfolio@mail.com");
            var realuser1 = await userManager.FindByEmailAsync("dawood.rizwan@outlook.com");
            var realuser2 = await userManager.FindByEmailAsync("natalieaktas@hotmail.com");
            var customer1 = await userManager.FindByEmailAsync("customer@mail.com");
            var customer2 = await userManager.FindByEmailAsync("customer2@mail.com");

            if (superadmin == null)
            {
                superadmin = new User
                {
                    UserName = "admin@mail.com",
                    Email = "admin@mail.com",
                    EmailConfirmed = true,
                    FirstName = "Admin",
                    LastName = "Adminsson"
                };
                await userManager.CreateAsync(superadmin, "Admin_2024");
                await userManager.AddToRoleAsync(superadmin, "superadmin");
            }

            if (useradmin == null)
            {
                useradmin = new User
                {
                    UserName = "portfolio@mail.com",
                    Email = "portfolio@mail.com",
                    EmailConfirmed = true,
                    FirstName = "Port",
                    LastName = "Folio"
                };
                await userManager.CreateAsync(useradmin, "Admin_2024");
                await userManager.AddToRoleAsync(useradmin, "admin");
            }

            if (realuser1 == null)
            {
                realuser1 = new User
                {
                    UserName = "dawood.rizwan@outlook.com",
                    Email = "dawood.rizwan@outlook.com",
                    EmailConfirmed = true,
                    FirstName = "Dawood",
                    LastName = "Rizwan"
                };
                await userManager.CreateAsync(realuser1, "Admin_2024");
                await userManager.AddToRoleAsync(realuser1, "employee");
            }

            if (realuser2 == null)
            {
                realuser2 = new User
                {
                    UserName = "natalieaktas@hotmail.com",
                    Email = "natalieaktas@hotmail.com",
                    EmailConfirmed = true,
                    FirstName = "Natalie",
                    LastName = "Aktas"
                };
                await userManager.CreateAsync(realuser2, "Admin_2024");
                await userManager.AddToRoleAsync(realuser2, "employee");
            }

            if (customer1 == null)
            {
                customer1 = new User
                {
                    UserName = "customer@mail.com",
                    Email = "customer@mail.com",
                    EmailConfirmed = true,
                    FirstName = "My",
                    LastName = "Work"
                };
                await userManager.CreateAsync(customer1, "Admin_2024");
                await userManager.AddToRoleAsync(customer1, "customer");
            }

            if (customer2 == null)
            {
                customer2 = new User
                {
                    UserName = "customer2@mail.com",
                    Email = "customer2@mail.com",
                    EmailConfirmed = true,
                    FirstName = "This",
                    LastName = "Work"
                };
                await userManager.CreateAsync(customer2, "Admin_2024");
                await userManager.AddToRoleAsync(customer2, "customer");
            }
        }

    }
}