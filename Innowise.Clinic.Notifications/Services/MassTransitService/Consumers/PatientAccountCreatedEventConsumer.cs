using Innowise.Clinic.Notifications.Constants;
using Innowise.Clinic.Notifications.Services.DocumentBuilderService;
using Innowise.Clinic.Notifications.Services.MailService.Interfaces;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;
using MassTransit;

namespace Innowise.Clinic.Notifications.Services.MassTransitService.Consumers;

public class PatientAccountCreatedEventConsumer : IConsumer<PatientAccountCreatedEvent>
{
    private readonly IEmailHandler _emailHandler;
    private readonly IDocumentBuilderService _documentBuilderService;

    public PatientAccountCreatedEventConsumer(IEmailHandler emailHandler, IDocumentBuilderService documentBuilderService)
    {
        _emailHandler = emailHandler;
        _documentBuilderService = documentBuilderService;
    }

    public async Task Consume(ConsumeContext<PatientAccountCreatedEvent> context)
    {
        var emailBody =
            _documentBuilderService.BuildBodyForEmailConfirmation(context.Message.EmailConfirmationLink);
        await _emailHandler.SendMessageAsync(context.Message.Email, EmailSubjects.EmailConfirmation, emailBody);
    }
}