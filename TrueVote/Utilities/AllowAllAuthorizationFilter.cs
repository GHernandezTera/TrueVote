using Hangfire.Dashboard;

namespace TrueVote.Utilities
{
    public class AllowAllAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true; 
        }
    }
}
