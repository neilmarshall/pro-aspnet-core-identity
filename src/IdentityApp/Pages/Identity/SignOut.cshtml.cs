using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Pages.Identity
{
    [AllowAnonymous]
    public class SignOutModel : UserPageModel
    {
        public SignInManager<IdentityUser<int>> SignInManager { get; private set; }

        public SignOutModel(SignInManager<IdentityUser<int>> signInMgr)
        {
            SignInManager = signInMgr;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await SignInManager.SignOutAsync();

            return RedirectToPage();
        }
    }
}
