namespace Cart.Api.Interfaces;

public interface IPdfService 
{
    byte[] CreatePdf(string htmlContent);
}