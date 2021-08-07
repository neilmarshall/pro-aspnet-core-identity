using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Pages.Identity
{
    public class UserDeleteModel : UserPageModel
    {
        public UserManager<IdentityUser<int>> UserManager { get; set; }

        public SignInManager<IdentityUser<int>> SignInManager { get; set; }

        public UserDeleteModel(UserManager<IdentityUser<int>> userManager, SignInManager<IdentityUser<int>> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await UserManager.GetUserAsync(User);

            var result = await UserManager.DeleteAsync(user);

            if (result.Process(ModelState))
            {
                await SignInManager.SignOutAsync();
                return Challenge();
            }

            return Page();
        }
    }
}
