using Innowise.Clinic.Auth.Services.Constants.Email;
using Innowise.Clinic.Notifications.MailService.Data;
using Innowise.Clinic.Notifications.MailService.Interfaces;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Innowise.Clinic.Notifications.MailService.Implementations;

public class EmailHandler : IEmailHandler
{
    private readonly IOptions<SmtpSettings> _smtpSettings;

    public EmailHandler(IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings;
    }

    public async Task SendMessageAsync(string mailRecipientMailAddress, string mailSubject, string mailBody)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_smtpSettings.Value.AuthenticationEmailSenderName,
            _smtpSettings.Value.AuthenticationEmailSenderAddress));
        message.To.Add(new MailboxAddress(mailRecipientMailAddress, mailRecipientMailAddress));
        message.Subject = mailSubject;
        message.Body = new TextPart("html")
        {
            Text = mailBody
        };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(_smtpSettings.Value.SmtpServerHost, _smtpSettings.Value.SmtpServerPort, false);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }

    public async Task SendEmailConfirmationLinkAsync(PatientAccountCreatedEvent emailConfirmationInfo)
    {
        var emailBody = EmailBodyBuilder.BuildBodyForEmailConfirmation(emailConfirmationInfo.EmailConfirmationLink);
        await SendMessageAsync(emailConfirmationInfo.Email, EmailSubjects.EmailConfirmation, emailBody);
    }

    public async Task SendEmailWithCredentialsAsync(EmployeeAccountGeneratedEvent credentials)
    {
        var emailBody = EmailBodyBuilder.BuildBodyWithCredentials(credentials);
        await SendMessageAsync(credentials.Email, EmailSubjects.AdminProfileRegistration, emailBody);
    }
}