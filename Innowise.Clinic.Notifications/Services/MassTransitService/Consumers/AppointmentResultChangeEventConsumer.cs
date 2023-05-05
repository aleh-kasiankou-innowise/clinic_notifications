using Innowise.Clinic.Notifications.Constants;
using Innowise.Clinic.Notifications.Services.DataSyncService;
using Innowise.Clinic.Notifications.Services.DocumentBuilderService;
using Innowise.Clinic.Notifications.Services.HtmlToPdfService;
using Innowise.Clinic.Notifications.Services.MailService.Interfaces;
using Innowise.Clinic.Shared.Constants;
using Innowise.Clinic.Shared.Enums;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Requests;
using MassTransit;
using Serilog;

namespace Innowise.Clinic.Notifications.Services.MassTransitService.Consumers;

public class AppointmentResultNotificationEventConsumer : IConsumer<AppointmentResultNotification>
{
    private readonly IEmailHandler _emailHandler;
    private readonly IDataService _dataService;
    private readonly IDocumentBuilderService _documentBuilderService;
    private readonly IHtmlToPdfService _pdfService;
    private readonly IRequestClient<BlobSaveRequest> _blobSaveRequestClient;

    public AppointmentResultNotificationEventConsumer(IEmailHandler emailHandler, IDataService dataSyncService,
        IDocumentBuilderService documentBuilderService, IHtmlToPdfService pdfService,
        IRequestClient<BlobSaveRequest> blobSaveRequestClient)
    {
        _emailHandler = emailHandler;
        _dataService = dataSyncService;
        _documentBuilderService = documentBuilderService;
        _pdfService = pdfService;
        _blobSaveRequestClient = blobSaveRequestClient;
    }

    public async Task Consume(ConsumeContext<AppointmentResultNotification> context)
    {
        Log.Debug("{MassTransitActionType} result notification event for appointment with id {AppointmentId}",
            "Consuming", context.Message.AppointmentId);
        var appointmentDetails = await _dataService.GetAppointmentExtendedInfoAsync(
            context.Message.DoctorId,
            context.Message.PatientId,
            context.Message.ServiceId
        );

        var emailBody = _documentBuilderService.BuildBodyWithAppointmentResult(context.Message, appointmentDetails);
        var pdfDocument = _pdfService.GeneratePdf(emailBody);
        var isSentFirstTime = await _dataService.TryGetValueFromCache(string.Concat(
            RedisKeys.AppointmentResult.ToString(),
            context.Message.AppointmentId.ToString()), "1") is null;

        var actionType = isSentFirstTime ? AppointmentResultChangeType.Add : AppointmentResultChangeType.Update;
        await _blobSaveRequestClient.GetResponse<BlobSaveResponse>(new(context.Message.AppointmentId,
            BlobCategories.AppointmentResultPdf,
            pdfDocument, "application/pdf"));
        await _emailHandler.SendMessageAsync(context.Message.PatientId.ToString(),
            EmailSubjects.AppointmentResultUpdated, emailBody);
    }
}