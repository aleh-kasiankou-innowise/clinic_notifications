using Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;

namespace Innowise.Clinic.Notifications.MailService.Interfaces;

public interface IEmailHandler
{
    Task SendMessageAsync(string mailRecipient, string mailSubject, string mailBody);

    Task SendEmailConfirmationLinkAsync(PatientAccountCreatedEvent emailConfirmationInfo);

    Task SendEmailWithCredentialsAsync(EmployeeAccountGeneratedEvent credentials);
}