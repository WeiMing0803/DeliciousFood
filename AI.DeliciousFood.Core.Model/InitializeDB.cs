using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AI.DeliciousFood.Core.Model;

public class InitializeDB
{
    public static async Task InitializeDatabase(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<FoodRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<FoodUser>>();
        var roles = new[] { "Admin", "User", "VIPUser" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var newRole = new FoodRole
                {
                    Name = role
                };
                await roleManager.CreateAsync(newRole);
            }
        }

        var adminUser = new FoodUser
        {
            UserName = "admin",
            Email = "admin@example.com",
            EmailConfirmed = true
        };

        var existingAdminUser = await userManager.FindByNameAsync(adminUser.UserName);
        if (existingAdminUser == null)
        {
            var result = await userManager.CreateAsync(adminUser, "AdminPassword123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
