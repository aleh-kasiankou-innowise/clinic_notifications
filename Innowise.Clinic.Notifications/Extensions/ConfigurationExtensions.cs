using Innowise.Clinic.Notifications.MailService.Data;
using Innowise.Clinic.Notifications.MailService.Implementations;
using Innowise.Clinic.Notifications.MailService.Interfaces;
using Innowise.Clinic.Notifications.MassTransitService.Consumers;
using MassTransit;

namespace Innowise.Clinic.Notifications.Extensions;

public static class ConfigurationExtensions
{
    public static IServiceCollection ConfigureCrossServiceCommunication(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqConfig = configuration.GetSection("RabbitConfigurations");

        services.AddMassTransit(x =>
        {
            x.AddConsumer<EmployeeAccountGeneratedEventConsumer>();
            x.AddConsumer<PatientAccountCreatedEventConsumer>();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqConfig["HostName"], h =>
                {
                    h.Username(rabbitMqConfig["UserName"]);
                    h.Password(rabbitMqConfig["Password"]);
                });
                cfg.ConfigureEndpoints(context);
            });
        });
        return services;
    }

    public static IServiceCollection ConfigureSmtp(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SmtpSettings>(configuration.GetSection("Smtp"));
        services.AddSingleton<IEmailHandler, EmailHandler>();
        return services;
    }
}