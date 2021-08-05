using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Pages.Identity.Admin
{
    public class ViewModel : AdminPageModel
    {
        public UserManager<IdentityUser<int>> UserManager { get; set; }

        public IdentityUser<int> IdentityUser { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        public IEnumerable<string> PropertyNames
            => typeof(IdentityUser<int>).GetProperties().Select(prop => prop.Name);

        public string GetValue(string name)
            => typeof(IdentityUser<int>).GetProperty(name).GetValue(IdentityUser)?.ToString();

        public ViewModel(UserManager<IdentityUser<int>> mgr)
        {
            UserManager = mgr;
        }

        public async Task<IActionResult> OnGet()
        {
            IdentityUser = !string.IsNullOrEmpty(Id) ? await UserManager.FindByIdAsync(Id) : null;

            if (IdentityUser == null)
            {
                return RedirectToPage("SelectUser", new { Label = "View User", Callback = "View" });
            }

            return Page();
        }
    }
}
