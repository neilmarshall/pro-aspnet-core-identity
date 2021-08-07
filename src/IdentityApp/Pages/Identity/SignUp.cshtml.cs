using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IdentityApp.Pages.Identity
{
    [AllowAnonymous]
    public class SignUpModel : UserPageModel
    {
        public UserManager<IdentityUser<int>> UserManager { get; }
        public SignInManager<IdentityUser<int>> SignInManager { get; }

        [BindProperty]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        public string Password { get; set; }

        [BindProperty]
        [Compare(nameof(Password))]
        public string Confirmation { get; set; }

        public SignUpModel(
            UserManager<IdentityUser<int>> userManager,
            SignInManager<IdentityUser<int>> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
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

                var result = await UserManager.CreateAsync(user);

                if (result.Process(ModelState))
                {
                    result = await UserManager.AddPasswordAsync(user, Password);

                    if (!result.Process(ModelState))
                    {
                        await UserManager.DeleteAsync(user);
                    }
                }

                await SignInManager.SignInAsync(user, true);
            }

            return RedirectToPage("/");
        }
    }
}
