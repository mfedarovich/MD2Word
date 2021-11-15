using System;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using MD2Word.Word.Blocks;
using PlantUml.Net;
using Svg;

namespace MD2Word.Word
{
    public class DocImage : IImage
    {
        private enum ImageType
        {
            Svg,
            Uml,
            Png
        }
        private readonly OpenXmlElement _parent;
        private readonly EmbeddedImage _image;

        public DocImage(WordprocessingDocument doc, OpenXmlElement parent)
        {
            _parent = parent;
            _image = new EmbeddedImage(doc, 500);
        }
        public void InsertImageFromFile(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            switch (extension)
            {
                case ".png": 
                    InsertPngImage(File.ReadAllBytes(fileName)); 
                    break;
                case ".puml": 
                    InsertUml(File.ReadAllText(fileName));
                    break;
                default: throw new FileFormatException("Only png files are supported"); 
            }
        }

        public void InsertImageFromUrl(string url)
        {
            using var webClient = new WebClient();
            var data = webClient.DownloadData(url);
            switch (GetImageType(data))
            {
                case ImageType.Svg:
                    InsertSvgImage(data);
                    break;
                case ImageType.Uml:
                    InsertUml(data);
                    break;
                case ImageType.Png:
                    InsertPngImage(data);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static ImageType GetImageType(byte[] data)
        {
            var body = Encoding.UTF8.GetString(data, 0, Math.Min(10, data.Length));
            if (body.Contains("svg")) return ImageType.Svg;
            return body.Contains("@start") ? 
                ImageType.Uml : 
                ImageType.Png;
        }

        private void InsertSvgImage(byte[] data)
        {
            using var loadStream = new MemoryStream(data);
            var svgDocument = SvgDocument.Open<SvgDocument>(loadStream);
            var bitmap = svgDocument.Draw();
            using var saveStream = new MemoryStream();
            bitmap.Save(saveStream, ImageFormat.Png);
            InsertPngImage(saveStream.ToArray());
        }

        private void InsertUml(byte[] data)
        {
            using var memoryStream = new MemoryStream(data);
            using var reader = new StreamReader(memoryStream);
            InsertUml(reader.ReadToEnd());
        }
        public void InsertUml(string umlScript)
        {
            var factory = new RendererFactory();
            var plantUmlRenderer = factory.CreateRenderer(new PlantUmlSettings());
            var buffer = plantUmlRenderer.Render(umlScript, OutputFormat.Png);
            InsertPngImage(buffer);
        }

        private void InsertPngImage(byte[] buffer)
        {
            _image.AddImage(_parent, buffer);
        }

        public void Dispose()
        {
        }
    }
}