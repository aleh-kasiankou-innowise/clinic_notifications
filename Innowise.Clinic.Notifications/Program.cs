using Hangfire;
using Innowise.Clinic.Notifications.Extensions;
using Innowise.Clinic.Notifications.Services.SchedulingHelperService.Dashboard;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureCrossServiceCommunication(builder.Configuration);
builder.Services.ConfigureSmtp(builder.Configuration);
builder.Services.ConfigureCache(builder.Configuration);
builder.Services.ConfigureCron(builder.Configuration);
builder.Services.ConfigureServices();
builder.ConfigureSerilog();

var app = builder.Build();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new []{ new AllowGuestAccessToDashboardFilter()}
});

Log.Information("The Notifications service is starting");
app.Run();
Log.Information("The Notification service is stopping");
await Log.CloseAndFlushAsync();