using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace IdentityApp.Pages.Identity.Admin
{
    public class RolesModel : AdminPageModel
    {
        public UserManager<IdentityUser<int>> UserManager { get; private set; }
        public RoleManager<IdentityRole<int>> RoleManager { get; private set; }

        public string DashboardRole { get; }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        public IList<string> CurrentRoles { get; set; } = new List<string>();

        public IList<string> AvailableRoles { get; set; } = new List<string>();

        public RolesModel(
            UserManager<IdentityUser<int>> userManager,
            RoleManager<IdentityRole<int>> roleManager,
            IConfiguration configuration)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            DashboardRole = configuration["Dashboard:Role"] ?? "Dashboard";
        }

        private async Task SetProperties()
        {
            var user = await UserManager.FindByIdAsync(Id);

            CurrentRoles = await UserManager.GetRolesAsync(user);

            AvailableRoles = RoleManager.Roles.Select(r => r.Name).Where(r => !CurrentRoles.Contains(r)).ToList();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(Id))
            {
                return RedirectToPage("Selectuser", new { Label = "Edit Roles", Callback = "Roles" });
            }

            await SetProperties();

            return Page();
        }

        public async Task<IActionResult> OnPostAddToList(string role)
        {
            var result = await RoleManager.CreateAsync(new IdentityRole<int>(role));

            if (result.Process(ModelState))
            {
                return RedirectToPage();
            }

            await SetProperties();

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteFromList(string role)
        {
            var roleToDelete = await RoleManager.FindByNameAsync(role);

            var result = await RoleManager.DeleteAsync(roleToDelete);

            if (result.Process(ModelState))
            {
                return RedirectToPage();
            }

            await SetProperties();

            return Page();
        }

        public async Task<IActionResult> OnPostAdd([Required] string role)
        {
            if (ModelState.IsValid)
            {
                var result = IdentityResult.Success;

                if (result.Process(ModelState))
                {
                    var user = await UserManager.FindByIdAsync(Id);

                    if (!await UserManager.IsInRoleAsync(user, role))
                    {
                        result = await UserManager.AddToRoleAsync(user, role);
                    }

                    if (result.Process(ModelState))
                    {
                        return RedirectToPage();
                    }
                }
            }

            await SetProperties();

            return Page();
        }

        public async Task<IActionResult> OnPostDelete([Required] string role)
        {
            var user = await UserManager.FindByIdAsync(Id);

            if (await UserManager.IsInRoleAsync(user, role))
            {
                await UserManager.RemoveFromRoleAsync(user, role);
            }

            return RedirectToPage();
        }
    }
}
