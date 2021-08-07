using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Pages.Identity.Admin
{
    public class ClaimsModel : AdminPageModel
    {
        public UserManager<IdentityUser<int>> UserManager { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        public IEnumerable<Claim> Claims { get; set; }

        public ClaimsModel(UserManager<IdentityUser<int>> userManager)
        {
            UserManager = userManager;
        }

        public IEnumerable<(string type, string display)> AppClaimTypes =>
            ApplicationClaimTypes.AppClaimTypes.Where(ct => !Claims.Select(c => c.Type).Contains(ct.type));

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(Id))
            {
                return RedirectToPage("Selectuser", new { Label = "Manager Claims", Callback = "Claims" });
            }

            var user = await UserManager.FindByIdAsync(Id);

            Claims = await UserManager.GetClaimsAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(
            [Required] string task,
            [Required] string type,
            [Required] string value,
            string oldValue)
        {
            var user = await UserManager.FindByIdAsync(Id);

            Claims = await UserManager.GetClaimsAsync(user);

            if (ModelState.IsValid)
            {
                var claim = new Claim(type, value);

                var result = IdentityResult.Success;
                switch (task)
                {
                    case "add":
                        result = await UserManager.AddClaimAsync(user, claim);
                        break;
                    case "change":
                        result = await UserManager.ReplaceClaimAsync(user, new Claim(type, oldValue), claim);
                        break;
                    case "delete":
                        result = await UserManager.RemoveClaimAsync(user, claim);
                        break;
                }

                if (result.Process(ModelState))
                {
                    return RedirectToPage();
                }
            }

            return Page();
        }
    }
}
