using Hangfire;
using Innowise.Clinic.Notifications.Constants;
using Innowise.Clinic.Notifications.Services.DataSyncService;
using Innowise.Clinic.Notifications.Services.DocumentBuilderService;
using Innowise.Clinic.Notifications.Services.MailService.Interfaces;
using Innowise.Clinic.Notifications.Services.SchedulingHelperService;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;
using MassTransit;
using Serilog;

namespace Innowise.Clinic.Notifications.Services.MassTransitService.Consumers;

public class AppointmentNotificationEventConsumer : IConsumer<AppointmentNotification>
{
    private readonly IEmailHandler _emailHandler;
    private readonly IDataService _dataSyncService;
    private readonly IDocumentBuilderService _documentBuilderService;
    private readonly ISchedulingHelperService _schedulingHelperService;

    public AppointmentNotificationEventConsumer(IEmailHandler emailHandler, IDataService dataSyncService,
        ISchedulingHelperService schedulingHelperService, IDocumentBuilderService documentBuilderService)
    {
        _emailHandler = emailHandler;
        _dataSyncService = dataSyncService;
        _schedulingHelperService = schedulingHelperService;
        _documentBuilderService = documentBuilderService;
    }

    public async Task Consume(ConsumeContext<AppointmentNotification> context)
    {
        Log.Debug("{MassTransitActionType} reminder notification event for appointment with id {AppointmentId}",
            "Consuming", context.Message.AppointmentId);

        var appointmentDetails = await _dataSyncService.GetAppointmentExtendedInfoAsync(
            context.Message.DoctorId,
            context.Message.PatientId,
            context.Message.ServiceId
        );

        var emailBody =
            _documentBuilderService.BuildBodyWithAppointmentReminder(context.Message, appointmentDetails);

        if (_schedulingHelperService.IsAppointmentReminderDueForScheduling(context.Message, out var sendAfterDelay))
        {
            Log.Debug("Sending reminder notification for appointment with id {AppointmentId} without scheduling",
                context.Message.AppointmentId);
            await _emailHandler.SendMessageAsync(context.Message.PatientId.ToString(),
                EmailSubjects.AppointmentReminder,
                emailBody);
        }
        else
        {
            Log.Debug("Scheduling reminder notification for appointment with id {AppointmentId}",
                context.Message.AppointmentId);
            BackgroundJob.Schedule(() =>
                    _emailHandler.SendMessageAsync(context.Message.PatientId.ToString(),
                        EmailSubjects.AppointmentReminder,
                        emailBody)
                , sendAfterDelay);
        }
    }
}