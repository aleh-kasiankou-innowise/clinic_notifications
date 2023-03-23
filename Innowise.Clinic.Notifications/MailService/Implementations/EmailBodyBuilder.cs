using System.Text;
using Innowise.Clinic.Auth.Services.Constants.Email;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;

namespace Innowise.Clinic.Notifications.MailService.Implementations;

public static class EmailBodyBuilder
{
    public static string BuildBodyForEmailConfirmation(string emailConfirmationLink)
    {
        var stringBuilder = new StringBuilder(EmailTemplates.EmailConfirmation);
        stringBuilder.Replace(EmailVariables.EmailConfirmationLink, emailConfirmationLink);
        return stringBuilder.ToString();
    }

    public static string BuildBodyWithCredentials(EmployeeAccountGeneratedEvent userCredentials)
    {
        var stringBuilder = new StringBuilder(EmailTemplates.EmailWithCredentials);
        stringBuilder.Replace(EmailVariables.EmailAddress, userCredentials.Email);
        stringBuilder.Replace(EmailVariables.Password, userCredentials.Password);
        stringBuilder.Replace(EmailVariables.Role, userCredentials.Role);
        return stringBuilder.ToString();
    }
}