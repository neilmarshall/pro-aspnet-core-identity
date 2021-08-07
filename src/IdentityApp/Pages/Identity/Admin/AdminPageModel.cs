using Microsoft.AspNetCore.Authorization;

namespace IdentityApp.Pages.Identity.Admin
{
    [Authorize]
    public class AdminPageModel : UserPageModel
    {
    }
}
