using System;
using System.Collections.Generic;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MD2Word.Word.Extensions;

namespace MD2Word.Word.Blocks
{
    public class DocParagraph : DocBlockText, IParagraph
    {
        private Paragraph Paragraph => (Paragraph)Parent;

        public DocParagraph(WordprocessingDocument document, Paragraph paragraph, Dictionary<FontStyles, string> styles, Action onDestroy) : 
            base(document, paragraph, styles, onDestroy)
        {
        }

        public DocParagraph(WordprocessingDocument document, Paragraph paragraph, DocStyle style, Action onDestroy) : 
            base(document, paragraph, style, onDestroy)
        {
        }

        public override void SetStyle(FontStyles style, int level)
        {
            base.SetStyle(style, level);
            Paragraph.ApplyStyleId(Document.FindStyleIdByName(Style.Name));
        }

        public void CreateHorizontalRule()
        {
            ParagraphProperties paraProperties = new ();
            ParagraphBorders paraBorders = new ();
            paraBorders.Append(
                new BottomBorder() { Val = BorderValues.Single, Color = "auto", Size = 12U, Space = 1U },
                new BetweenBorder() {Val = BorderValues.Single, Color = "auto", Size = 12U, Space = 1U});
            paraProperties.AppendChild(paraBorders);
            Paragraph.AppendChild(paraProperties);
            Paragraph.AppendChild(new Run());
        }

        public override void WriteText(string text)
        {
            var run = Paragraph.AppendChild(new Run());
            run.Emphasise(Style.Italic, Style.Bold);
            var newChild = new Text(text);
            run.AppendChild(newChild);

        }
    }
}