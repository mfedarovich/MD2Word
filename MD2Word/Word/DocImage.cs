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
        private readonly OpenXmlElement _parent;
        private readonly EmbeddedImage _image;

        public DocImage(WordprocessingDocument doc, OpenXmlElement parent)
        {
            _parent = parent;
            _image = new EmbeddedImage(doc, 500);
        }
        public void InsertImageFromFile(string fileName)
        {
            if (Path.GetExtension(fileName) != ".png")
                throw new FileFormatException("Only png files are supported");
            
            InsertPngImage(File.ReadAllBytes(fileName));
        }

        public void InsertImageFromUrl(string url)
        {
            using var webClient = new WebClient();
            var data = webClient.DownloadData(url);
            if (IsSvg(data))
                InsertSvgImage(data);
            else
                InsertPngImage(data);
        }

        private bool IsSvg(byte[] data)
        {
            return Encoding.UTF8.GetString(data, 0, Math.Min(10, data.Length)).Contains("svg");
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