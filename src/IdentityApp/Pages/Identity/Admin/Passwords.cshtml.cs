using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Pages.Identity.Admin
{
    public class PasswordsModel : AdminPageModel
    {
        public UserManager<IdentityUser<int>> UserManager { get; private set; }

        public IdentityUser<int> IdentityUser { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        [BindProperty]
        [Required]
        public string Password { get; set; }

        [BindProperty]
        [Compare(nameof(Password))]
        public string Confirmation { get; set; }

        public PasswordsModel(UserManager<IdentityUser<int>> userManager)
        {
            UserManager = userManager;
        }

        public async Task<IActionResult> OnGet()
        {
            if (string.IsNullOrEmpty(Id))
            {
                return RedirectToPage("Selectuser", new { Label = "Password", Callback = "Passwords" });
            }

            IdentityUser = await UserManager.FindByIdAsync(Id);

            return Page();
        }

        public async Task<IActionResult> OnPostSetPasswordAsync()
        {
            if (ModelState.IsValid)
            {
                IdentityUser = await UserManager.FindByIdAsync(Id);

                if (await UserManager.HasPasswordAsync(IdentityUser))
                {
                    await UserManager.RemovePasswordAsync(IdentityUser);
                }

                var result = await UserManager.AddPasswordAsync(IdentityUser, Password);

                if (result.Process(ModelState))
                {
                    TempData["message"] = "Password Changed";

                    return RedirectToPage();
                }
            }

            return Page();
        }
    }
}
