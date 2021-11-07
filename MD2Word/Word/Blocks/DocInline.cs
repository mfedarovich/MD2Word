using System.Collections.Generic;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MD2Word.Word.Extensions;

namespace MD2Word.Word.Blocks
{
    public class DocInline : DocBlockText, IInline
    {
        public DocInline(WordprocessingDocument document, OpenXmlElement parent, Dictionary<FontStyles, string> styles) : base(document, parent, styles,
            () => {})
        {
        }
        
        public override void WriteText(string text)
        {
            var run = new Run();
            run.ApplyInlineStyle(base.Document, Style)
                .Emphasise(Style.Italic, Style.Bold)
                .AppendText(text, true);
            Parent.AppendChild(run);
        }
    }
}