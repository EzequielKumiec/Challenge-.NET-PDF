
# API de Extracción de Texto desde PDF e Imágenes

Esta API permite cargar archivos de imagen (JPEG, PNG, JPG) para extraer texto utilizando OCR (Reconocimiento Óptico de Caracteres) o la extracción directa de texto de archivos PDF.

## Características

- **Subir Archivos PDF**: Soporta la carga de archivos `.pdf` y extrae texto directamente si está disponible.
- **OCR para Imágenes**: Soporta la carga de archivos de imagen `.jpg`, `.jpeg` y `.png`. La API utiliza Tesseract OCR para extraer texto de las imágenes.
- **Manejo de Errores**: Maneja formatos de archivo no válidos, archivos vacíos y fallos en la extracción de texto.

## Requisitos

- **.NET Core 6+**: Esta API está construida con .NET Core.
- **Motor Tesseract**: Para el procesamiento OCR de imágenes y archivos PDF basados en imágenes.
- **iText7**: Para manejar el procesamiento de documentos PDF, incluyendo la extracción de texto desde PDFs nativos.
- **PdfSharp**: Para manejo adicional de archivos PDF (si es necesario).
- **XUnit**: Para pruebas unitarias.
- **Moq**: Un marco de simulación para las pruebas unitarias.

### Herramientas y Bibliotecas

- **Tesseract**: Un motor de OCR popular.
- **iText7**: Una biblioteca de .NET para leer y manipular archivos PDF, utilizada para la extracción de texto de PDFs nativos.
- **PdfSharp**: Una biblioteca de .NET para manejar documentos PDF (opcional, según los casos de uso específicos).
- **XUnit**: Para pruebas unitarias.
- **Moq**: Un marco de simulación para pruebas unitarias.

## Comenzando

### Clonar el Repositorio

```bash
git clone https://github.com/EzequielKumiec/Challenge-.NET-PDF.git
cd Challenge-.NET-PDF
```

# Endpoints de la API

Los siguientes endpoints están disponibles:

### POST `/upload`

- **Descripción**: Sube un archivo PDF o de imagen al servidor y extrae el texto.
- **Solicitud**: El archivo debe enviarse como datos de formulario en el cuerpo de la solicitud.
- **Formatos de Archivo Soportados**: `.pdf`, `.jpg`, `.jpeg`, `.png`.
- **Respuesta**: Devuelve el texto extraído del archivo.

#### Ejemplo de Solicitud (usando curl)

```bash
curl -X POST "http://localhost:5000/upload" -F "file=@path/to/your/file.pdf"
```

#### Archivos de Prueba

Para fines de prueba, he incluido dos archivos de ejemplo en la carpeta `Files` que puedes cargar en la API:

- **PDF de prueba.pdf**: Un documento PDF de ejemplo que se puede cargar para probar la funcionalidad de extracción de PDF.
- **Imagen de prueba.jpg**: Un archivo de imagen de ejemplo para probar la extracción con OCR.

Puedes cargar estos archivos directamente a través del endpoint `/upload`.

#### Pruebas

Las pruebas unitarias para el `UploadController` se pueden ejecutar usando el siguiente comando:

```bash
dotnet test
```

Las pruebas verificarán la funcionalidad de carga para diferentes casos:

- Formato de archivo no válido (archivos que no sean PDF ni imágenes).
- Carga de archivo vacío.
- Extracción exitosa de texto tanto desde archivos PDF como desde imágenes.
