using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Pages.Identity.Admin
{
    public class SelectUserModel : AdminPageModel
    {
        public UserManager<IdentityUser<int>> UserManager { get; set; }

        public IEnumerable<IdentityUser<int>> Users { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Label { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Callback { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Filter { get; set; }

        public SelectUserModel(UserManager<IdentityUser<int>> mgr)
        {
            UserManager = mgr;
        }

        public void OnGet()
        {
            Users = UserManager.Users
                .Where(user => Filter == null || user.Email.Contains(Filter))
                .OrderBy(user => user.Email)
                .ToList();
        }

        public IActionResult OnPost()
            => RedirectToPage(new { Filter, Callback });
    }
}
