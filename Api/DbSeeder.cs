using Microsoft.AspNetCore.Identity;
using TournamentManager.Application.Common;
using TournamentManager.Domain.Entities;

namespace TournamentManager.Api
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            try
            {
                // Seed roles
                string[] roles = [Roles.Admin, Roles.Captain, Roles.Player];
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        var result = await roleManager.CreateAsync(new IdentityRole(role));
                        if (!result.Succeeded)
                            Console.WriteLine($"Failed to create role {role}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }

                // Seed default admin user
                if (!userManager.Users.Any())
                {
                    var admin = new ApplicationUser
                    {
                        UserName = "hello@ipekbayrak.dev",
                        Email = "hello@ipekbayrak.dev",
                        FirstName = "İpek",
                        LastName = "Bayrak",
                        DisplayName = "İpek",
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(admin, "Kusursuzipek0$");
                    if (result.Succeeded)
                        await userManager.AddToRoleAsync(admin, Roles.Admin);
                    else
                        Console.WriteLine($"Failed to create admin: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding failed: {ex.Message}");
            }
        }
    }
}
