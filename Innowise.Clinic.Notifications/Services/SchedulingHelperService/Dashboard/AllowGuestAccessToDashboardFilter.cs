using Hangfire.Dashboard;

namespace Innowise.Clinic.Notifications.Services.SchedulingHelperService.Dashboard;

public class AllowGuestAccessToDashboardFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true;
    }
}