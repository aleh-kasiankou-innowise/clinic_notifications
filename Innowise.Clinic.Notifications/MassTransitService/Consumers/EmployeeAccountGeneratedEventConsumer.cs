using Innowise.Clinic.Notifications.MailService.Interfaces;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;
using MassTransit;

namespace Innowise.Clinic.Notifications.MassTransitService.Consumers;

public class EmployeeAccountGeneratedEventConsumer : IConsumer<EmployeeAccountGeneratedEvent>
{
    private readonly IEmailHandler _emailHandler;

    public EmployeeAccountGeneratedEventConsumer(IEmailHandler emailHandler)
    {
        _emailHandler = emailHandler;
    }

    public async Task Consume(ConsumeContext<EmployeeAccountGeneratedEvent> context)
    {
        await _emailHandler.SendEmailWithCredentialsAsync(context.Message);
    }
}