namespace Innowise.Clinic.Notifications.Services.HtmlToPdfService;

public interface IHtmlToPdfService
{
    byte[] GeneratePdf(string html);
}