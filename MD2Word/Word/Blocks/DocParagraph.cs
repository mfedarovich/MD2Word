using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MD2Word.Word.Extensions;

namespace MD2Word.Word.Blocks
{
    public class DocParagraph : DocBlockText, IParagraph
    {
        private readonly Paragraph _paragraph;

        public DocParagraph(WordprocessingDocument document, Paragraph paragraph, Dictionary<FontStyles, string> styles) : base(document, paragraph, styles)
        {
            _paragraph = paragraph;
        }

        public override void SetStyle(FontStyles style, int level)
        {
            base.SetStyle(style, level);
            _paragraph.ApplyStyleId(Document.FindStyleIdByName(Style.Name));
        }

        public override void WriteText(string text)
        {
            var run = _paragraph.AppendChild(new Run());
            run.Emphasise(Style.Italic, Style.Bold);
            var newChild = new Text(text);
            run.AppendChild(newChild);

        }
    }
}