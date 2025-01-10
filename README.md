# PDF and Image Text Extraction API

This API allows the upload of PDF and image files (JPEG, PNG, JPG) to extract text using OCR (Optical Character Recognition) or direct PDF text extraction.

## Features

- **Upload PDF Files**: Supports the upload of `.pdf` files and extracts text directly if available.
- **OCR for Images**: Supports uploading `.jpg`, `.jpeg`, and `.png` image files. The API uses Tesseract OCR to extract text from images.
- **OCR for PDF Images**: Extracts text from image-based PDFs using Tesseract OCR.
- **Error Handling**: Handles invalid file formats, empty files, and failed text extraction.

## Requirements

- **.NET Core 6+**: This API is built with .NET Core.
- **Tesseract Engine**: For OCR processing of images and image-based PDFs.
- **iText7**: For handling PDF document processing, including text extraction from native PDFs.
- **PdfSharp**: For additional PDF handling (if needed).
- **XUnit**: For unit testing.
- **Moq**: A mocking framework for unit tests.

### Tools and Libraries

- **Tesseract**: A popular OCR engine.
- **iText7**: A .NET library for reading and manipulating PDF files, used for text extraction from native PDFs.
- **PdfSharp**: A .NET library for handling PDF documents (optional, based on specific use cases).
- **XUnit**: For unit testing.
- **Moq**: A mocking framework for unit tests.

## Getting Started

### Clone the Repository

```bash
git clone https://github.com/EzequielKumiec/Challenge-.NET-PDF.git
cd Challenge-.NET-PDF
```
# API Endpoints

The following endpoints are available:

### POST `/upload`

- **Description**: Uploads a PDF or image file to the server and extracts text.
- **Request**: The file should be sent as form data in the request body.
- **Supported File Formats**: `.pdf`, `.jpg`, `.jpeg`, `.png`.
- **Response**: Returns the extracted text from the file.

#### Example Request (using curl)

```bash
curl -X POST "http://localhost:5000/upload" -F "file=@path/to/your/file.pdf"
```
#### Test Files
For testing purposes, I have included two example files in the Files folder that you can upload to the API:

- **PDF de prueba.pdf**: A sample PDF document that can be uploaded to test PDF extraction functionality.
- **Imagen de prueba.jpg**: A sample image file for testing OCR extraction.
You can upload these files directly via the /upload endpoint.

#### Testing
Unit tests for the UploadController can be run using the following commands:

Run Tests:

```bash
dotnet test
```
