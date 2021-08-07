using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Pages.Identity.Admin
{
    public class LockoutsModel : AdminPageModel
    {
        public UserManager<IdentityUser<int>> UserManager { get; }

        public IEnumerable<IdentityUser<int>> LockedOutUsers { get; set; }

        public IEnumerable<IdentityUser<int>> OtherUsers { get; set; }

        public LockoutsModel(UserManager<IdentityUser<int>> userManager)
        {
            UserManager = userManager;
        }

        public async Task<TimeSpan> TimeLeft(IdentityUser<int> user) =>
            (await UserManager.GetLockoutEndDateAsync(user)).GetValueOrDefault().Subtract(DateTimeOffset.Now);

        public void OnGet()
        {
            LockedOutUsers = UserManager
                .Users
                .Where(user => user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.Now)
                .OrderBy(user => user.Email)
                .ToList();

            OtherUsers = UserManager
                .Users
                .Where(user => !user.LockoutEnd.HasValue || user.LockoutEnd.Value <= DateTimeOffset.Now)
                .OrderBy(user => user.Email)
                .ToList();
        }

        public async Task<IActionResult> OnPostLockAsync(string id)
        {
            var user = await UserManager.FindByIdAsync(id);

            await UserManager.SetLockoutEnabledAsync(user, true);

            await UserManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now.AddMinutes(1));

            await UserManager.UpdateSecurityStampAsync(user);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUnlockAsync(string id)
        {
            var user = await UserManager.FindByIdAsync(id);

            await UserManager.SetLockoutEndDateAsync(user, null);

            return RedirectToPage();
        }
    }
}
