using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace IdentityApp.Pages.Identity
{
    public class IndexModel : UserPageModel
    {
        public string Email { get; set; }
        public string Phone { get; set; }

        public UserManager<IdentityUser<int>> UserManager { get; private set; }

        public IndexModel(UserManager<IdentityUser<int>> userManager)
        {
            UserManager = userManager;
        }

        public async Task OnGetAsync()
        {
            var currentUser = await UserManager.GetUserAsync(User);

            Email = currentUser?.Email ?? "(No Value)";
            Phone = currentUser?.PhoneNumber ?? "(No Value)";
        }
    }
}
