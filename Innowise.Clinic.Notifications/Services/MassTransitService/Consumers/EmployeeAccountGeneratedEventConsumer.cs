using Innowise.Clinic.Notifications.Constants;
using Innowise.Clinic.Notifications.Services.DocumentBuilderService;
using Innowise.Clinic.Notifications.Services.MailService.Interfaces;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;
using MassTransit;

namespace Innowise.Clinic.Notifications.Services.MassTransitService.Consumers;

public class EmployeeAccountGeneratedEventConsumer : IConsumer<EmployeeAccountGeneratedEvent>
{
    private readonly IDocumentBuilderService _documentBuilderService;
    private readonly IEmailHandler _emailHandler;

    public EmployeeAccountGeneratedEventConsumer(IEmailHandler emailHandler,
        IDocumentBuilderService documentBuilderService)
    {
        _emailHandler = emailHandler;
        _documentBuilderService = documentBuilderService;
    }

    public async Task Consume(ConsumeContext<EmployeeAccountGeneratedEvent> context)
    {
        var emailBody = _documentBuilderService.BuildBodyWithCredentials(context.Message);
        await _emailHandler.SendMessageAsync(context.Message.Email, EmailSubjects.AdminProfileRegistration, emailBody);
    }
}