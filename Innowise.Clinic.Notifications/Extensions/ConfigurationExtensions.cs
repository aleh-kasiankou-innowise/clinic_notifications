using DinkToPdf;
using DinkToPdf.Contracts;
using Hangfire;
using Hangfire.Redis.StackExchange;
using Innowise.Clinic.Notifications.Services.DataSyncService;
using Innowise.Clinic.Notifications.Services.DocumentBuilderService;
using Innowise.Clinic.Notifications.Services.HtmlToPdfService;
using Innowise.Clinic.Notifications.Services.MailService.Data;
using Innowise.Clinic.Notifications.Services.MailService.Implementations;
using Innowise.Clinic.Notifications.Services.MailService.Interfaces;
using Innowise.Clinic.Notifications.Services.MassTransitService.Consumers;
using Innowise.Clinic.Notifications.Services.SchedulingHelperService;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Requests;
using MassTransit;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Innowise.Clinic.Notifications.Extensions;

public static class ConfigurationExtensions
{
    public static IServiceCollection ConfigureCrossServiceCommunication(this IServiceCollection services,
        IConfiguration configuration)
    {
        var rabbitMqConfig = configuration.GetSection("RabbitConfigurations");

        services.AddMassTransit(x =>
        {
            x.AddConsumer<EmployeeAccountGeneratedEventConsumer>();
            x.AddConsumer<PatientAccountCreatedEventConsumer>();
            x.AddConsumer<AppointmentNotificationEventConsumer>();
            x.AddConsumer<AppointmentResultNotificationEventConsumer>();
            x.AddRequestClient<PatientNameRequest>();
            x.AddRequestClient<DoctorNameRequest>();
            x.AddRequestClient<ServiceNameRequest>();
            x.AddRequestClient<BlobSaveRequest>();
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

    public static IServiceCollection ConfigureCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDistributedRedisCache(x => x.Configuration = configuration.GetConnectionString("Redis"));
        return services;
    }

    public static IServiceCollection ConfigureCron(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(x => x.UseRedisStorage(configuration.GetConnectionString("Redis")));
        services.AddHangfireServer();
        return services;
    }
    
    public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder)
    {
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(builder.Configuration["ElasticSearchHost"]))
            {
                AutoRegisterTemplate = true,
                OverwriteTemplate = true,
                IndexFormat = $"clinic.notifications-{0:yy.MM}",
                BatchAction = ElasticOpType.Index,
                DetectElasticsearchVersion = true,
            })
            .WriteTo.Console()
            .CreateLogger();

        Log.Logger = logger;
        builder.Host.UseSerilog(logger);
        return builder;
    }

    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddSingleton<IConverter>(new SynchronizedConverter(new PdfTools()));
        services.AddSingleton<IDocumentBuilderService, DocumentBuilderService>();
        services.AddSingleton<IHtmlToPdfService, HtmlToPdfService>();
        services.AddSingleton<ISchedulingHelperService, SchedulingHelperService>();
        services.AddSingleton<IDataService, DataService>();
        return services;
    }
}