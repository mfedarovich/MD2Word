using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MD2Word.Word;
using PlantUml.Net;

namespace MD2Word
{
    public class Document : IDocument
    {
        private readonly WordprocessingDocument _doc;
        private Paragraph _paragraph;
        private readonly EmbeddedImage _image;
        private readonly DocStyle _style = new();

        public Document(WordprocessingDocument doc)
        {
            _doc = doc;
            _image = new EmbeddedImage(doc, 500);
        }

        public void StartNextParagraph()
        {
            _paragraph = CreateParagraphAfter(_paragraph);
            _paragraph.ApplyStyleId(_doc.FindStyleIdByName(_style.ParagraphName));
        }

        public TextWriter GetWriter()
        {
            return new DocumentWriter(this);
        }
        
        public void WriteText(string text)
        {
            AppendText(_paragraph, text);
        }
        
        public void WriteInlineText(string text)
        {
            var run = new Run();
            run.ApplyInlineStyle(_doc, _style)
                .Emphasise(_style.Italic, _style.Bold)
                .AppendText(text, true);
            _paragraph.AppendChild(run);
        }

        public void WriteSymbol(string htmlSymbol)
        {
            var symbol = HtmlSymbol.Parse(htmlSymbol);
            var run = new Run();
            run
                .ApplyInlineStyle(_doc, _style)
                .Emphasise(_style.Italic, _style.Bold)
                .AppendSymbol(symbol);
            
            _paragraph.AppendChild(run);
        }

        public void WriteLine()
        {
            _paragraph.AppendChild(new Run(new Break()));
        }
        public void WriteHyperlink(string url)
        {
            WriteHyperlink(url,url);
        }
        public void WriteHyperlink(string label, string url)
        {
            Uri uri;
            try
            {
                uri = new Uri(url);
            }
            catch (UriFormatException e)
            {
                WriteInlineText(label);
                WriteInlineText(url);
                return;
            }
            var mainPart = _doc.MainDocumentPart;
            var rel = mainPart!.HyperlinkRelationships.FirstOrDefault(hr => hr.Uri == uri) ??
                      mainPart.AddHyperlinkRelationship(uri, true);

            var run = new Run(
                new RunProperties(
                    new RunStyle() { Val = "Hyperlink" }),
                new Text(label)
            );
            run.Emphasise(_style.Italic, _style.Bold);
            
            var hl = new Hyperlink(
                new ProofError() { Type = ProofingErrorValues.GrammarStart },
                run) { History = OnOffValue.FromBoolean(true), Id = rel.Id };
            _paragraph.AppendChild(hl);
        }

        public void Emphasise(bool italic, bool bold)
        {
            _style.Bold = bold;
            _style.Italic = italic;
        }

        public void WriteHtml(string html)
        {
            _paragraph = CreateParagraphAfter(_paragraph);
            string altChunkId = $"codeId_{html.GetHashCode()}";
            // var run = new Run(new Text("test"));
            // var p = new Paragraph(new ParagraphProperties(
            //         new Justification() { Val = JustificationValues.Center }),
            //     run);

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(html));

            // Create alternative format import part.
            var formatImportPart = _doc.MainDocumentPart?.AddAlternativeFormatImportPart(
                    AlternativeFormatImportPartType.Html, altChunkId);
            //ms.Seek(0, SeekOrigin.Begin);

            // Feed HTML data into format import part (chunk).
            formatImportPart.FeedData(ms);
            AltChunk altChunk = new AltChunk();
            altChunk.Id = altChunkId;
            _paragraph.Append(altChunk);
        }

        public void PushStyle(FontStyles style, bool inline)
        {
            _style.Push(style, inline);
        }

        public void PushStyle(FontStyles style, int nestingLevel)
        {
            _style.Push(style, nestingLevel);
        }

        public void PopStyle(bool inline)
        {
            _style.Pop(inline);
        }

        public void InsertPngImage(byte[] buffer)
        {
            _paragraph = CreateParagraphAfter(_paragraph);
            _image.AddImage(_paragraph, buffer);
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
            InsertPngImage(data);
        }

        public void InsertUml(string umlScript)
        {
            var factory = new RendererFactory();
            var plantUmlRenderer = factory.CreateRenderer(new PlantUmlSettings());
            var buffer = plantUmlRenderer.Render(umlScript, OutputFormat.Png);
            InsertPngImage(buffer);
        }

        private Paragraph CreateParagraphAfter(OpenXmlElement? element)
        {
            bool removePlaceholder = false;
            if (element == null)
            {
                element = _doc.GetBodyPlaceholder();
                removePlaceholder = true;
            }

            var paragraph = element.InsertAfterSelf(new Paragraph());

            if (removePlaceholder)
                element.Remove();
            return paragraph;
        }
        
        private void AppendText(OpenXmlElement element, string text)
        {
            var run = element.AppendChild(new Run());
            run.Emphasise(_style.Italic, _style.Bold);

            var newChild = new Text(text);
            run.AppendChild(newChild);
        }
    }
}