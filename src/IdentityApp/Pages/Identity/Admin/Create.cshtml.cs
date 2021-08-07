using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Pages.Identity.Admin
{
    public class CreateModel : AdminPageModel
    {
        public UserManager<IdentityUser<int>> UserManager { get; private set; }

        [BindProperty]
        [EmailAddress]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        public string Password { get; set; }

        [BindProperty]
        [Compare(nameof(Password))]
        public string Confirmation { get; set; }

        public CreateModel(UserManager<IdentityUser<int>> mgr)
        {
            UserManager = mgr;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser<int>
                {
                    UserName = Email,
                    Email = Email,
                    EmailConfirmed = true
                };

                user.PasswordHash = UserManager.PasswordHasher.HashPassword(user, Password);

                var result = await UserManager.CreateAsync(user);

                if (result.Process(ModelState))
                {
                    TempData["message"] = "Account Created";

                    return RedirectToPage();
                }
            }

            return Page();
        }
    }
}
