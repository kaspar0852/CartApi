using Cart.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cart.Api.Controllers.PdfContoller;

[Route("api/[controller]")]
public class PdfController : ControllerBase
{
    private readonly IPdfService _pdfService;

    public PdfController(IPdfService pdfService)
    {
        _pdfService = pdfService;
    }

    [HttpPost("convertToPdf")]
    public async Task<bool> ConvertToPdfAsync()
    {
        try
        {
            string htmlFilePath = "/Users/kaspar/Desktop/dotnet/HtmlToPdfApp/index 4 (1).html";
            string outputPdfPath = "output.pdf";

            // Read the HTML content
            string htmlContent = System.IO.File.ReadAllText(htmlFilePath);


            _pdfService.CreatePdf(htmlContent);

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}