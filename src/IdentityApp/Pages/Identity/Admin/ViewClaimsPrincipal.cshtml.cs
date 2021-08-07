using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Pages.Identity.Admin
{
    public class ViewClaimsPrincipalModel : AdminPageModel
    {
        public UserManager<IdentityUser<int>> UserManager { get; }

        public SignInManager<IdentityUser<int>> SignInManager { get; }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Callback { get; set; }

        public ClaimsPrincipal Principal { get; set; }

        public ViewClaimsPrincipalModel(
            UserManager<IdentityUser<int>> userManager,
            SignInManager<IdentityUser<int>> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(Id))
            {
                return RedirectToPage("Selectuser", new { Label = "View ClaimsPrincipal", Callback = "ClaimsPrincipal" });
            }

            var user = await UserManager.FindByIdAsync(Id);

            Principal = await SignInManager.CreateUserPrincipalAsync(user);

            return Page();
        }
    }
}
