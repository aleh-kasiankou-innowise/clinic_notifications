using DinkToPdf;
using DinkToPdf.Contracts;

namespace Innowise.Clinic.Notifications.Services.HtmlToPdfService;

public class HtmlToPdfService : IHtmlToPdfService
{
    private readonly IConverter _pdfConverter;

    public HtmlToPdfService(IConverter pdfConverter)
    {
        _pdfConverter = pdfConverter;
    }

    public byte[] GeneratePdf(string html)
    {
        var globalPdfConfig = new GlobalSettings()
        {
            ColorMode = ColorMode.Grayscale,
            Orientation = Orientation.Portrait,
            PaperSize = PaperKind.A4,
        };

        var pdfObject = new ObjectSettings()
        {
            PagesCount = true,
            HtmlContent = html,
            WebSettings = { DefaultEncoding = "utf-8" },
            HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 }
        };

        return _pdfConverter.Convert(new HtmlToPdfDocument { GlobalSettings = globalPdfConfig, Objects = { pdfObject }});
    }
}