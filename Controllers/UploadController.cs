using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Tesseract;
using iText.Kernel.Pdf.Xobject;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PDF_Reader.Controllers
{
    [Route("")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly TesseractEngine _tesseractEngine;

        public UploadController()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var tessDataPath = Path.Combine(baseDirectory, "..", "..", "..", "tessdata");

            _tesseractEngine = new TesseractEngine(tessDataPath, "spa", EngineMode.Default);
        }


        [HttpPost("upload")]
        public IActionResult Upload(IFormFile file)
        {
            
            
            if (file == null || file.Length == 0)
                return BadRequest("File not uploaded.");

            string extractedText = string.Empty;

            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (fileExtension == ".pdf")
            {
                using (var pdfStream = new MemoryStream())
                {
                    file.CopyTo(pdfStream);
                    extractedText = ExtractTextFromPdf(pdfStream);
                }

                if (string.IsNullOrWhiteSpace(extractedText))
                {
                    extractedText = ExtractTextUsingOCR(file);
                }
            }
            else if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png")
            {
                extractedText = ExtractTextUsingOCRForImage(file);
            }
            else
            {
                return BadRequest("Please upload a valid PDF or JPG file.");
            }

            if (string.IsNullOrWhiteSpace(extractedText))
            {
                return BadRequest("Failed to extract text from the file.");
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
                    var strategy = new SimpleTextExtractionStrategy();
                    for (int i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
                    {
                        var page = pdfDocument.GetPage(i);
                        var pageText = PdfTextExtractor.GetTextFromPage(page, strategy);
                        pageText = pageText.Replace("\r\n", " ").Replace("\n", " ");
                        textBuilder.Append(pageText);
                    }
                }
                return textBuilder.ToString();
            }
            catch (Exception ex)
            {
                return $"Error al extraer texto: {ex.Message}";
            }
        }

        private string ExtractTextUsingOCR(IFormFile file)
        {
            try
            {
                using (var pdfReader = new PdfReader(file.OpenReadStream()))
                using (var pdfDocument = new PdfDocument(pdfReader))
                {
                    var textBuilder = new StringBuilder();

                    for (int i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
                    {
                        var page = pdfDocument.GetPage(i);
                        var images = ExtractImagesFromPage(page);

                        foreach (var image in images)
                        {
                            using (var pix = Pix.LoadFromMemory(image))
                            {
                                var result = _tesseractEngine.Process(pix);
                                textBuilder.Append(result.GetText());
                            }
                        }
                    }

                    var cleanText = textBuilder.ToString();

                    return cleanText;
                }
            }
            catch (Exception ex)
            {
                return $"Error al extraer texto: {ex.Message}";
            }
        }

        private string ExtractTextUsingOCRForImage(IFormFile file)
        {
            try
            {
                using (var stream = file.OpenReadStream())
                {
                    using (var pix = Pix.LoadFromMemory(ReadFully(stream)))
                    {
                        var result = _tesseractEngine.Process(pix);
                        string cleanedText = result.GetText().Replace("\r\n", " ").Replace("\n", " ");
                        return cleanedText;
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error al extraer texto: {ex.Message}";
            }
        }

        private List<byte[]> ExtractImagesFromPage(PdfPage page)
        {
            var images = new List<byte[]>();

            try
            {
                var resources = page.GetResources();
                var xObjectNames = resources.GetResourceNames();

                foreach (var xObjectName in xObjectNames)
                {
                    var pdfObject = resources.GetResource(xObjectName);

                    if (pdfObject is PdfDictionary pdfDictionary && pdfDictionary.Get(PdfName.Subtype)?.ToString() == "/Image")
                    {
                        try
                        {
                            var imageObj = new PdfImageXObject((PdfStream)pdfDictionary);

                            images.Add(imageObj.GetImageBytes());
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error procesando la imagen: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error extrayendo imágenes: " + ex.Message);
            }

            return images;
        }

        private byte[] ReadFully(Stream input)
        {
            using (var ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
