using System;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MD2Word.Commands;

namespace MD2Word.Word.Styles
{
    public class SetStyleCommand : ICommand
    {
        private readonly WordprocessingDocument _document;
        private readonly Paragraph _paragraph;
        private readonly string _styleName;

        public SetStyleCommand(WordprocessingDocument document, Paragraph paragraph, string styleName)
        {
            _document = document;
            _paragraph = paragraph;
            _styleName = styleName;
        }
        public void Execute()
        {
            var styleId = GetStyleIdFromStyleName(_document, _styleName);
            ApplyStyleToParagraph(styleId);
        }
        
        public static string GetStyleIdFromStyleName(WordprocessingDocument doc, string styleName)
        {
            string styleId = null;
            
            var stylePart = doc.MainDocumentPart?.StyleDefinitionsPart;
            if (stylePart?.Styles != null)
            {
                styleId = stylePart.Styles.Descendants<StyleName>()
                    .Where(s => string.Compare(s.Val?.Value, styleName, StringComparison.OrdinalIgnoreCase) == 0  &&
                                ((Style) s.Parent)?.Type == StyleValues.Paragraph)
                    .Select(n => ((Style) n.Parent)?.StyleId).FirstOrDefault();
            }

            if (styleId == null)
                throw new Exception($"Style \"{styleName}\" is not found");
            
            return styleId;
        }

        
        private void ApplyStyleToParagraph(string styleId)
        {
            var pPr = _paragraph.Elements<ParagraphProperties>().FirstOrDefault() ?? 
                      _paragraph.PrependChild(new ParagraphProperties());
            pPr.ParagraphStyleId = new ParagraphStyleId(){Val = styleId};
        }
    }
}