using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private readonly Stack<Tuple<string, bool>> _stack = new Stack<Tuple<string, bool>>();
        private readonly WordprocessingDocument _doc;
        private Paragraph _paragraph;
        private readonly EmbeddedImage _image;

        public Document(WordprocessingDocument doc)
        {
            _doc = doc;
            _image = new EmbeddedImage(doc, 500);
            _stack.Push(new Tuple<string, bool>("Body Text", false)); 
        }

        public void StartNextParagraph()
        {
            _paragraph = CreateParagraphAfter(_paragraph);
            var styleName = Style;
            if (Inline)
                styleName = _stack.AsEnumerable().Reverse().FirstOrDefault(x => !x.Item2)?.Item1;

            if (styleName != null)
                _paragraph.ApplyStyleId(_doc.FindStyleIdByName(styleName));
        }

        public TextWriter GetWriter()
        {
            return new DocumentWriter(this);
        }
        
        private string Style => _stack.Peek().Item1;
        private bool Inline => _stack.Peek().Item2;
        
        public void WriteText(string text)
        {
            AppendText(_paragraph, text);
        }
        
        public void WriteInlineText(string text)
        {
            var run = _paragraph.AppendChild(new Run());
            if (Inline)
            {
                run.ApplyStyleId(_doc.FindStyleIdByName(Style, false));
            }
            
            var newChild = new Text(text);
            newChild.Space = new EnumValue<SpaceProcessingModeValues>(SpaceProcessingModeValues.Preserve);
            run.AppendChild(newChild);
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

        public void PushStyle(string style, bool inline)
        {
            _stack.Push(new Tuple<string, bool>(style, inline));
            // var pPr = _paragraph.Elements<ParagraphProperties>().FirstOrDefault();
            // _stack.Push(pPr?.ParagraphStyleId?.Val);
        }

        public void PopStyle()
        {
            // var styleId =
            _stack.Pop();
        //     if (styleId != null)
        //     {
        //         _paragraph = CreateParagraphAfter(_paragraph);
        //         ApplyStyleId(styleId);
        //     }
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

        public void InsertUml(string umlScript)
        {
            var factory = new RendererFactory();
            var plantUmlRenderer = factory.CreateRenderer(new PlantUmlSettings());
            var buffer = plantUmlRenderer.Render(umlScript, OutputFormat.Png);
            InsertPngImage(buffer);
        }

        public void WriteHyperlink(string url)
        {
            var hyperlink = new Hyperlink();
            _paragraph.AppendChild(hyperlink);
            AppendText(hyperlink, url);
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

            if (text == Environment.NewLine)
                run.AppendChild(new Break());
            else
            {
                var newChild = new Text(text);
                run.AppendChild(newChild);
            }
        }

    }
}