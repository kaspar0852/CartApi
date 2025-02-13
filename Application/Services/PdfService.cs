using Cart.Api.Interfaces;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace Cart.Api.Application.Services;

public class PdfService : IPdfService
{
    private readonly IConverter _converter;

    public PdfService(IConverter converter)
    {
        _converter = converter;
    }
    
    public byte[] CreatePdf(string htmlContent)
    {


        var globalSettings = new GlobalSettings
        {
            ColorMode = ColorMode.Color,
            Orientation = Orientation.Portrait,
            PaperSize = PaperKind.A4Plus,
            Margins = new MarginSettings { Top = 10 },
            DocumentTitle = "PDF Report",
        };

        var webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var destinationDirectoryPath = Path.Combine(webRootPath, "Assets");

        var objectSettings = new ObjectSettings
        {
            PagesCount = true,
            HtmlContent = htmlContent,
            WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(destinationDirectoryPath, "PDFstyle.css") },
            HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = false },
            FooterSettings = { FontName = "Arial", FontSize = 9, Line = false, Center = " " }
        };

        var pdf = new HtmlToPdfDocument()
        {
            GlobalSettings = globalSettings,
            Objects = { objectSettings }
        };

        //_converter.Convert(pdf); // for download
        //for pdf view in browser
        var file = _converter.Convert(pdf);

        return file;

    }
}