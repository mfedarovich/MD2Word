using System;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace MD2Word.Word.Styles
{
    public class SetStyleByNameCommand : SetStyleByIdCommand
    {
        public SetStyleByNameCommand(WordprocessingDocument document, Paragraph paragraph, string styleName) :
            base (document, paragraph, GetStyleIdFromStyleName(document, styleName))
        {
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
    }
}