using Innowise.Clinic.Notifications.MailService.Interfaces;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;
using MassTransit;

namespace Innowise.Clinic.Notifications.MassTransitService.Consumers;

public class PatientAccountCreatedEventConsumer : IConsumer<PatientAccountCreatedEvent>
{
    private readonly IEmailHandler _emailHandler;

    public PatientAccountCreatedEventConsumer(IEmailHandler emailHandler)
    {
        _emailHandler = emailHandler;
    }

    public async Task Consume(ConsumeContext<PatientAccountCreatedEvent> context)
    {
        await _emailHandler.SendEmailConfirmationLinkAsync(context.Message);
    }
}