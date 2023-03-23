using Innowise.Clinic.Notifications.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureCrossServiceCommunication(builder.Configuration);
builder.Services.ConfigureSmtp(builder.Configuration);

var app = builder.Build();
app.Run();