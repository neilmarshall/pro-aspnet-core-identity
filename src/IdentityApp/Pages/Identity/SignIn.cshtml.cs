using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Pages.Identity
{
    [AllowAnonymous]
    public class SignInModel : UserPageModel
    {
        public SignInManager<IdentityUser<int>> SignInManager { get; private set; }

        [Required]
        [EmailAddress]
        [BindProperty]
        public string Email { get; set; }

        [Required]
        [BindProperty]
        public string Password { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        public SignInModel(SignInManager<IdentityUser<int>> signInMgr)
        {
            SignInManager = signInMgr;
        }

        public static void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await SignInManager.PasswordSignInAsync(Email, Password, true, true);

                if (result.Succeeded)
                {
                    return Redirect(ReturnUrl ?? "/");
                }
                else if (result.IsLockedOut)
                {
                    TempData["message"] = "Account Locked";
                }
                else if (result.IsNotAllowed)
                {
                    TempData["message"] = "Sign In Not Allowed";
                }
                else if (result.RequiresTwoFactor)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    TempData["message"] = "Sign In Failed";
                }
            }

            return Page();
        }
    }
}
