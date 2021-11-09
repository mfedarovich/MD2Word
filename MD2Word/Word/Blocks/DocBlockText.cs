using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MD2Word.Word.Extensions;
using Break = DocumentFormat.OpenXml.Wordprocessing.Break;
using Hyperlink = DocumentFormat.OpenXml.Wordprocessing.Hyperlink;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using RunProperties = DocumentFormat.OpenXml.Wordprocessing.RunProperties;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;

namespace MD2Word.Word.Blocks
{
    public abstract class DocBlockText : IBlockText
    {
        protected WordprocessingDocument Document { get; }
        protected OpenXmlElement Parent { get; }
        public DocStyle Style { get; }

        public event Action? Closing;

        protected DocBlockText(WordprocessingDocument document, OpenXmlElement parent, Dictionary<FontStyles, string> styles)
        {
            Document = document;
            Parent = parent;
            Style = new DocStyle(styles);
        }
        
        public virtual void SetStyle(FontStyles style, int level)
        {
            Style.Style = style;
            Style.Level = level;
        }

        public void Emphasise(bool italic, bool bold)
        {
            Style.Italic = italic;
            Style.Bold = bold;
        }

        public abstract void WriteText(string text);

        public void WriteSymbol(string htmlSymbol)
        {
            var symbol = HtmlSymbol.Parse(htmlSymbol);
            var run = new Run();
            run
                .ApplyInlineStyle(Document, Style)
                .Emphasise(Style.Italic, Style.Bold)
                .AppendSymbol(symbol);
            
            Parent.AppendChild(run);
        }

        public  void WriteLine()
        {
            Parent.AppendChild(new Run(new Break()));
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
                WriteText(label);
                WriteText(url);
                return;
            }
            var mainPart = Document.MainDocumentPart;
            var rel = mainPart!.HyperlinkRelationships.FirstOrDefault(hr => hr.Uri == uri) ??
                      mainPart.AddHyperlinkRelationship(uri, true);

            var drawing = Parent.Descendants<Drawing>().FirstOrDefault();

            if (drawing != null)
            {
                var clone = drawing.CloneNode(true);
                var hl = new Hyperlink(new ProofError() { Type = ProofingErrorValues.GrammarStart }, clone)
                {
                    History = OnOffValue.FromBoolean(true), 
                    Id = rel.Id
                };
                drawing.InsertAfterSelf(hl);
                drawing.Remove();
                (Parent as Run)?.ApplyStyleId(Style[FontStyles.Hyperlink]);
            }
            else
            {
                var existing = Parent.Descendants<Run>().LastOrDefault();
                Run run;
                if (existing != null)
                {
                    run = (Run)existing.CloneNode(true);
                    existing.Remove();
                }
                else
                {
                    run = new Run(new Text(label));
                }
                run.Emphasise(Style.Italic, Style.Bold);
                run.ApplyStyleId(Style[FontStyles.Hyperlink]);

                var hl = new Hyperlink(new ProofError() { Type = ProofingErrorValues.GrammarStart }, run)
                {
                    History = OnOffValue.FromBoolean(true), 
                    Id = rel.Id
                };
                Parent.AppendChild(hl);
            }
        }
        
        // public void WriteHtml(string html)
        // {
        //     string altChunkId = $"codeId_{html.GetHashCode()}";
        //     // var run = new Run(new Text("test"));
        //     // var p = new Paragraph(new ParagraphProperties(
        //     //         new Justification() { Val = JustificationValues.Center }),
        //     //     run);
        //
        //     var ms = new MemoryStream(Encoding.UTF8.GetBytes(html));
        //
        //     // Create alternative format import part.
        //     var formatImportPart = _doc.MainDocumentPart?.AddAlternativeFormatImportPart(
        //         AlternativeFormatImportPartType.Html, altChunkId);
        //     //ms.Seek(0, SeekOrigin.Begin);
        //
        //     // Feed HTML data into format import part (chunk).
        //     formatImportPart?.FeedData(ms);
        //     AltChunk altChunk = new()
        //     {
        //         Id = altChunkId
        //     };
        //     Current.Append(altChunk);
        // }
        
        public void Dispose()
        {
            Closing?.Invoke();
        }
    }
}