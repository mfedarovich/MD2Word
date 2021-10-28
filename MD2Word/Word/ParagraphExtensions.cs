using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;

namespace MD2Word.Word
{
    public static class ParagraphExtensions
    {
        public static void ApplyStyleId(this Paragraph paragraph, string styleId)
        {
            var pPr = paragraph.Elements<ParagraphProperties>().FirstOrDefault() ?? 
                      paragraph.PrependChild(new ParagraphProperties());
            pPr.ParagraphStyleId = new ParagraphStyleId(){Val = styleId};
        }
    }
}