using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Pages.Identity.Admin
{
    public class DashboardModel : AdminPageModel
    {
        public int UsersCount { get; set; } = 0;
        public int UsersUnconfirmed { get; set; } = 0;
        public int UsersLockedOut { get; set; } = 0;
        public int UsersTwoFactor { get; set; } = 0;

        public UserManager<IdentityUser<int>> UserManager { get; private set; }

        private readonly string[] emails =
        {
            "alice_app2@example.com",
            "bob_app2@example.com",
            "charlie_app2@example.com"
        };

        public DashboardModel(UserManager<IdentityUser<int>> userManager)
        {
            UserManager = userManager;
        }

        public void OnGet()
        {
            UsersCount = UserManager.Users.Count();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            foreach (var existingUser in UserManager.Users.ToList())
            {
                if (existingUser.UserName.Contains("_app2"))
                {
                    var result = await UserManager.DeleteAsync(existingUser);
                    result.Process(ModelState);
                }
            }

            foreach (var email in emails)
            {
                var user = new IdentityUser<int>
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await UserManager.CreateAsync(user);
                result.Process(ModelState);
            }

            if (ModelState.IsValid)
                return RedirectToPage();

            return Page();
        }
    }
}
