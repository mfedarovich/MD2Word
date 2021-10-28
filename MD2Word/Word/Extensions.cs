using System;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace MD2Word.Word
{
    public static class Extensions
    {
        public static OpenXmlElement GetBodyPlaceholder(this WordprocessingDocument document)
        {
            // string contentControlTag;
            var element = document.MainDocumentPart?.Document.Body?.Descendants<SdtElement>().FirstOrDefault();
            // .FirstOrDefault(sdt => sdt.SdtProperties.GetFirstChild<Tag>()?.Val == contentControlTag);
            if (element == null)
                throw new ArgumentException($"Documentation body placeholder is not found.");

            return element;
        }

        public static void SetUpdateFieldsOnOpen(this WordprocessingDocument doc)
        {
            var settingsPart = doc.MainDocumentPart.DocumentSettingsPart;
            settingsPart.Settings.Append(new UpdateFieldsOnOpen() { Val = true });
        }
        public static string FindStyleIdByName(this WordprocessingDocument doc, string styleName, bool forParagraph = true)
        {         
            string? styleId = null;
            
            var stylePart = doc.MainDocumentPart?.StyleDefinitionsPart;
            if (stylePart?.Styles != null)
            {
                //var type = forParagraph ? StyleValues.Paragraph : StyleValues.Character;
                var type = StyleValues.Paragraph;
                var style = stylePart.Styles.Descendants<StyleName>()
                    .Where(s => string.Compare(s.Val?.Value, styleName, StringComparison.OrdinalIgnoreCase) == 0  &&
                                ((Style) s.Parent!)?.Type == type)
                    .Select(n => ((Style) n.Parent!)).FirstOrDefault();

                if (!forParagraph && style?.LinkedStyle != null)
                {
                    styleId = style.LinkedStyle.Val.Value;
                }
                else
                {
                    styleId = style?.StyleId;
                }
            }

            var array = stylePart.Styles.Descendants<StyleName>().Where(s =>
                string.Compare(s.Val?.Value, styleName, StringComparison.OrdinalIgnoreCase) == 0).ToArray();
            if (styleId == null)
                throw new Exception($"Style \"{styleName}\" is not found");
            
            return styleId;
        }
        
        public static void ApplyStyleId(this Paragraph paragraph, string styleId)
        {
            var pPr = paragraph.Elements<ParagraphProperties>().FirstOrDefault() ?? 
                      paragraph.PrependChild(new ParagraphProperties());
            pPr.ParagraphStyleId = new ParagraphStyleId(){Val = styleId};
        }

        public static void ApplyStyleId(this Run run, string styleId)
        {
            var pPr = run.Elements<RunProperties>().FirstOrDefault() ??
                      run.PrependChild(new RunProperties());
            pPr.RunStyle = new RunStyle { Val = styleId };
        }
    }
}