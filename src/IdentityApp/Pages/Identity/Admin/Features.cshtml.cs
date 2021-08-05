using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace IdentityApp.Pages.Identity.Admin
{
    public class FeaturesModel : AdminPageModel
    {
        public UserManager<IdentityUser<int>> UserManager { get; private set; }

        public IEnumerable<(string, string)> Features { get; set; }

        public FeaturesModel(UserManager<IdentityUser<int>> mgr)
        {
            UserManager = mgr;
        }

        public void OnGet()
        {
            Features = UserManager.GetType().GetProperties()
                .Where(prop => prop.Name.StartsWith("Supports"))
                .OrderBy(prop => prop.Name)
                .Select(prop => (prop.Name, prop.GetValue(UserManager).ToString()));
        }
    }
}
