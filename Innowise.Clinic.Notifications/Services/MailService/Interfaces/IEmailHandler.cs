namespace Innowise.Clinic.Notifications.Services.MailService.Interfaces;

public interface IEmailHandler
{
    Task SendMessageAsync(string mailRecipient, string mailSubject, string mailBody);
}