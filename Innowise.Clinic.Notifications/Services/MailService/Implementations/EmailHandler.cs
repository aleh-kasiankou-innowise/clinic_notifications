using Innowise.Clinic.Notifications.Services.DataSyncService;
using Innowise.Clinic.Notifications.Services.DocumentBuilderService;
using Innowise.Clinic.Notifications.Services.MailService.Data;
using Innowise.Clinic.Notifications.Services.MailService.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Innowise.Clinic.Notifications.Services.MailService.Implementations;

public class EmailHandler : IEmailHandler
{
    private readonly SmtpSettings _smtpSettings;
    private readonly IDocumentBuilderService _documentBuilderService;
    private readonly IDataService _dataSyncService;


    public EmailHandler(IOptions<SmtpSettings> smtpSettings, IDocumentBuilderService documentBuilderService,
        IDataService dataSyncService)
    {
        _documentBuilderService = documentBuilderService;
        _dataSyncService = dataSyncService;
        _smtpSettings = smtpSettings.Value;
    }

    public async Task SendMessageAsync(string mailRecipientMailAddress, string mailSubject, string mailBody)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_smtpSettings.AuthenticationEmailSenderName,
            _smtpSettings.AuthenticationEmailSenderAddress));
        message.To.Add(new MailboxAddress(mailRecipientMailAddress, mailRecipientMailAddress));
        message.Subject = mailSubject;
        message.Body = new TextPart("html")
        {
            Text = mailBody
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(_smtpSettings.SmtpServerHost, _smtpSettings.SmtpServerPort, false);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}