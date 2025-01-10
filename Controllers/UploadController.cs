using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Mvc;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PDF_Reader.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        [HttpPost("upload")]
        public IActionResult Upload( IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File not uploaded.");

            if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
            {
                return BadRequest("Please upload a valid PDF file.");
            }

            string extractedText;

            using (var pdfStream = new MemoryStream())
            {
                file.CopyTo(pdfStream);
                extractedText = ExtractTextFromPdf(pdfStream);
            }
            // Si no se extrajo texto, respondemos con un error
            if (string.IsNullOrWhiteSpace(extractedText))
            {
                return BadRequest("Failed to extract text from the PDF.");
            }

            return Ok(new { text = extractedText });
        }

        private string ExtractTextFromPdf(Stream pdfStream)
        {
            try
            {
                pdfStream.Position = 0;
                var textBuilder = new StringBuilder();
                using (var pdfReader = new PdfReader(pdfStream))
                using (var pdfDocument = new PdfDocument(pdfReader))
                {
                    var strategy = new iText.Kernel.Pdf.Canvas.Parser.Listener.SimpleTextExtractionStrategy();

                    for (int i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
                    {
                        var page = pdfDocument.GetPage(i);
                        var pageText = PdfTextExtractor.GetTextFromPage(page, strategy);

                        // Reemplazar saltos de línea por un espacio
                        pageText = pageText.Replace("\r\n", " ").Replace("\n", " ");

                        textBuilder.Append(pageText);
                    }
                }

                return textBuilder.ToString();
            }
            catch (Exception ex)
            {
                // Logear el error y retornar una respuesta de error adecuada
                return $"Error extracting text: {ex.Message}";
            }
        }
    }
}
