namespace Innowise.Clinic.Notifications.Services.MailService.Data;

public record SmtpSettings
{
    public string SmtpServerHost { get; set; }
    public int SmtpServerPort { get; set; }
    public string AuthenticationEmailSenderName { get; set; }
    public string AuthenticationEmailSenderAddress { get; set; }
}