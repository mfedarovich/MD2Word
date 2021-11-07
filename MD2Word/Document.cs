using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MD2Word.Word;
using MD2Word.Word.Tables;
using PlantUml.Net;

namespace MD2Word
{
    
    public class Document : IDocument
    {
        private readonly WordprocessingDocument _doc;
        private OpenXmlElement? _parent;
        private readonly EmbeddedImage _image;
        private readonly DocStyle _style;

        public Document(WordprocessingDocument doc, Dictionary<FontStyles, string> styles)
        {
            _doc = doc;
            _image = new EmbeddedImage(doc, 500);
            _style = new DocStyle(styles);
        }

        public ITable CreateTable()
        {
            return new DocTable(_doc.MainDocumentPart?.Document.Body!, (newParent) => _parent = newParent);
        }

        public void CreateParagraph()
        {
            var paragraph = CreateParagraphAfter(_parent);
            paragraph.ApplyStyleId(_doc.FindStyleIdByName(_style.ParagraphName));
            _parent = paragraph;
        }

        public TextWriter GetWriter()
        {
            return new DocumentWriter(this);
        }
        
        public void WriteText(string text)
        {
            AppendText(_parent!, text);
        }
        
        public void WriteInlineText(string text)
        {
            var run = new Run();
            run.ApplyInlineStyle(_doc, _style)
                .Emphasise(_style.Italic, _style.Bold)
                .AppendText(text, true);
            _parent?.AppendChild(run);
        }

        public void WriteSymbol(string htmlSymbol)
        {
            var symbol = HtmlSymbol.Parse(htmlSymbol);
            var run = new Run();
            run
                .ApplyInlineStyle(_doc, _style)
                .Emphasise(_style.Italic, _style.Bold)
                .AppendSymbol(symbol);
            
            _parent?.AppendChild(run);
        }

        public void WriteLine()
        {
            _parent?.AppendChild(new Run(new Break()));
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
            catch (UriFormatException)
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
                    new RunStyle() { Val = _style[FontStyles.Hyperlink] }),
                new Text(label)
            );
            run.Emphasise(_style.Italic, _style.Bold);
            
            var hl = new Hyperlink(
                new ProofError() { Type = ProofingErrorValues.GrammarStart },
                run) { History = OnOffValue.FromBoolean(true), Id = rel.Id };
            _parent?.AppendChild(hl);
        }

        public void Emphasise(bool italic, bool bold)
        {
            _style.Bold = bold;
            _style.Italic = italic;
        }

        public void WriteHtml(string html)
        {
            _parent = CreateParagraphAfter(_parent);
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
            formatImportPart?.FeedData(ms);
            AltChunk altChunk = new()
            {
                Id = altChunkId
            };
            _parent.Append(altChunk);
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
            _parent = CreateParagraphAfter(_parent);
            _image.AddImage(_parent, buffer);
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

        public void Dispose()
        {
            _doc?.Dispose();
        }
    }
}