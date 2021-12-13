using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MD2Word.Word.Extensions;

namespace MD2Word.Word.Blocks
{
    public class DocParagraph : DocBlockText, IParagraph
    {
        private readonly DocList _docList;
        private Paragraph Paragraph => (Paragraph)Parent;
        
        public DocParagraph(WordprocessingDocument document, Paragraph paragraph, IReadOnlyDictionary<FontStyles, string> styles, DocList docList) : 
            base(document, paragraph, styles)
        {
            _docList = docList;
        }
        
        public override void SetStyle(FontStyles style, int level)
        {
            base.SetStyle(style, level);
            if (style is FontStyles.BulletList or FontStyles.NumberList)
            {
                _docList.ApplyStyle(Paragraph, style, level);
            }
            else
            {
                Paragraph.ApplyStyleId(Document.FindStyleIdByName(Style.Name));
            }
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