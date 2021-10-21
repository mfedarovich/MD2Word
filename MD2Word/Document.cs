using System;
using System.IO;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MD2Word.Word.Styles;

namespace MD2Word
{
    public class Document : IDocument
    {
        private readonly WordprocessingDocument _doc;
        private Paragraph _paragraph;

        public Document(WordprocessingDocument doc)
        {
            _doc = doc;
        }

        public TextWriter GetWriter()
        {
            return new DocumentWriter(this);
        }

        public void AddNewBlock(string style)
        {
            _paragraph = CreateParagraphAfter(_paragraph);
            ApplyStyle(style);
        }
        
        public void WriteText(string text)
        {
            if(_paragraph == null) AddNewBlock("Body Text");
            var run = _paragraph.AppendChild(new Run());
            run.AppendChild(new Text(text));
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

        private Paragraph CreateParagraphAfter(OpenXmlElement? element)
        {
            bool removePlaceholder = false;
            if (element == null)
            {
                element = FindBodyPlaceholder();
                removePlaceholder = true;
            }

            var paragraph = element.InsertAfterSelf(new Paragraph());

            if (removePlaceholder)
                element.Remove();
            return paragraph;
        }

        private OpenXmlElement FindBodyPlaceholder()
        {
            // string contentControlTag;
            var element = _doc.MainDocumentPart?.Document.Body?.Descendants<SdtElement>().FirstOrDefault();
            // .FirstOrDefault(sdt => sdt.SdtProperties.GetFirstChild<Tag>()?.Val == contentControlTag);
            if (element == null)
                throw new ArgumentException($"Documentation body placeholder is not found.");

            return element;
        }
        
        private void ApplyStyle(string style)
        {
            var styleCommand = new SetStyleCommand(_doc, _paragraph, style);
            styleCommand.Execute();
        }



    }
}