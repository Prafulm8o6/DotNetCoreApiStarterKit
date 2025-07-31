using DotNetCoreApiStarterKit.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace DotNetCoreApiStarterKit.Infrastructure.Seed
{
    public class DefaultDataSeeder
    {
        public static async Task SeedDefaultDataAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            string[] roleNames = { "Admin", "User" };

            // Create roles if they don't exist
            foreach (var role in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Seed Admin User
            var adminEmail = "admin@example.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            // ✅ Seed Test User
            var testEmail = "testuser@example.com";
            var testUser = await userManager.FindByEmailAsync(testEmail);

            if (testUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "testuser",
                    Email = testEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "Test@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "User");
                }
            }
        }
    }
}
