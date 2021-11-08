using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;

namespace MD2Word.Word.Extensions
{
    public static class ParagraphExtensions
    {
        public static void ApplyStyleId(this Paragraph paragraph, string styleId)
        {
            var pPr = paragraph.Elements<ParagraphProperties>().FirstOrDefault() ?? 
                      paragraph.PrependChild(new ParagraphProperties());
            pPr.ParagraphStyleId = new ParagraphStyleId(){Val = styleId};
        }

        public static void Align(this Paragraph paragraph, CellAlignment align)
        {
            ParagraphProperties paraProperties = new();
            Justification justification = new();
            justification.Val = align switch
            {
                CellAlignment.Center => JustificationValues.Center,
                CellAlignment.Right => JustificationValues.Right,
                CellAlignment.Left => JustificationValues.Left,
                _ => justification.Val
            };

            paraProperties.Append(justification);
            paragraph.Append(paraProperties);
        }
    }
}