using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Pages.Identity.Admin
{
    public class DeleteModel : AdminPageModel
    {
        public UserManager<IdentityUser<int>> UserManager { get; set; }

        public IdentityUser<int> IdentityUser { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        public DeleteModel(UserManager<IdentityUser<int>> userManager)
        {
            UserManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(Id))
            {
                return RedirectToPage("Selectuser", new { Label = "Delete", Callback = "Delete" });
            }

            IdentityUser = await UserManager.FindByIdAsync(Id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            IdentityUser = await UserManager.FindByIdAsync(Id);

            var result = await UserManager.DeleteAsync(IdentityUser);

            if (result.Process(ModelState))
            {
                return RedirectToPage("Dashboard");
            }

            return Page();
        }
    }
}
