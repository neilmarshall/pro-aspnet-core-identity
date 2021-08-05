using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Pages.Identity.Admin
{
    public class EditBindingTarget
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class EditModel : AdminPageModel
    {
        public UserManager<IdentityUser<int>> UserManager { get; private set; }

        public IdentityUser<int> IdentityUser { get; private set; }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        public EditModel(UserManager<IdentityUser<int>> mgr)
        {
            UserManager = mgr;
        }

        public async Task<IActionResult> OnGet()
        {
            IdentityUser = !string.IsNullOrEmpty(Id) ? await UserManager.FindByIdAsync(Id) : null;

            if (IdentityUser == null)
            {
                return RedirectToPage("SelectUser", new { Label = "Edit User", Callback = "Edit" });
            }

            return Page();
        }

        public async Task<IActionResult> OnPost([FromForm(Name = "IdentityUser")] EditBindingTarget userData)
        {
            if (!string.IsNullOrEmpty(Id) && ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(Id);

                if (user != null)
                {
                    user.UserName = userData.Email;
                    user.Email = userData.Email;
                    user.EmailConfirmed = true;

                    var result = await UserManager.UpdateAsync(user);

                    if (result.Process(ModelState))
                    {
                        return RedirectToPage();
                    }
                }
            }

            IdentityUser = await UserManager.FindByIdAsync(Id);

            return Page();
        }
    }
}
