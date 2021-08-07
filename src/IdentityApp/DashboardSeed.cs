using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityApp
{
    public static class DashboardSeed
    {
        public static void SeedUserStoreForDashboard(this IApplicationBuilder app)
        {
            SeedStore(app).GetAwaiter().GetResult();
        }

        private async static Task SeedStore(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var config = scope.ServiceProvider.GetService<IConfiguration>();

            var userManager = scope.ServiceProvider.GetService<UserManager<IdentityUser<int>>>();

            var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole<int>>>();

            var roleName = config["Dashboard:Role"] ?? "Dashboard";
            var userName = config["Dashboard:User"] ?? "admin@example.com";
            var password = config["Dashboard:Password"] ?? "MySecret1$";

            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole<int>(roleName));
            }

            var dashboardUser = await userManager.FindByEmailAsync(userName);

            if (dashboardUser == null)
            {
                dashboardUser = new IdentityUser<int>
                {
                    UserName = userName,
                    Email = userName,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(dashboardUser);

                dashboardUser = await userManager.FindByEmailAsync(userName);

                await userManager.AddPasswordAsync(dashboardUser, password);
            }

            if (!await userManager.IsInRoleAsync(dashboardUser, roleName))
            {
                await userManager.AddToRoleAsync(dashboardUser, roleName);
            }
        }
    }
}