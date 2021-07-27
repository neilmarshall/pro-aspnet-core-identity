namespace IdentityApp.Pages.Identity.Admin
{
    public class DashboardModel : AdminPageModel
    {
        public int UsersCount { get; set; } = 0;
        public int UsersUnconfirmed { get; set; } = 0;
        public int UsersLockedOut { get; set; } = 0;
        public int UsersTwoFactor { get; set; } = 0;
    }
}
