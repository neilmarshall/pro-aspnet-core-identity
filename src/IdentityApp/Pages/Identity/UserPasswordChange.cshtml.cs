using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Pages.Identity
{
    public class PassworcChangeBindingTarget
    {
        [Required]
        public string Current { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }
    }

    public class UserPasswordChangeModel : UserPageModel
    {
        public UserManager<IdentityUser<int>> UserManager { get; private set; }

        public UserPasswordChangeModel(UserManager<IdentityUser<int>> userManager)
        {
            UserManager = userManager;
        }

        public async Task<IActionResult> OnPostAsync(PassworcChangeBindingTarget data)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.GetUserAsync(User);

                var result = await UserManager.ChangePasswordAsync(user, data.Current, data.NewPassword);

                if (result.Process(ModelState))
                {
                    TempData["message"] = "Password changed";

                    return RedirectToPage();
                }
            }

            return Page();
        }
    }
}
