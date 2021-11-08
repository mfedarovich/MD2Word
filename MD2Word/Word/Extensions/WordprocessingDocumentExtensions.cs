using System;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace MD2Word.Word.Extensions
{
    public static class Extensions
    {
        public static OpenXmlElement GetPlaceholder(this WordprocessingDocument document, string tag)
        {
            // string contentControlTag;
            var element = document.MainDocumentPart?.Document.Body?.Descendants<SdtElement>()
                .FirstOrDefault(sdt => sdt.SdtProperties?.GetFirstChild<Tag>()?.Val == tag);
            if (element == null)
                throw new ArgumentException($"Documentation body placeholder is not found.");

            return element;
        }

        public static void RemovePlaceholders(this WordprocessingDocument document)
        {
            var placeholders = document.MainDocumentPart?.Document.Body?.Descendants<SdtElement>().ToArray();
            foreach (var placeholder in placeholders)
            {
                placeholder.Remove();
            }
        }

        public static void SetUpdateFieldsOnOpen(this WordprocessingDocument doc)
        {
            var settingsPart = doc.MainDocumentPart?.DocumentSettingsPart;
            settingsPart?.Settings.AppendChild(new UpdateFieldsOnOpen { Val = OnOffValue.FromBoolean(true) });
        }
        public static string FindStyleIdByName(this WordprocessingDocument doc, string styleName, bool forParagraph = true)
        {         
            string? styleId = null;
            
            var stylePart = doc.MainDocumentPart?.StyleDefinitionsPart;
            if (stylePart?.Styles != null)
            {
                 var style = stylePart.Styles.Descendants<StyleName>()
                    .Where(s => string.Compare(s.Val?.Value, styleName, StringComparison.OrdinalIgnoreCase) == 0  &&
                                ((Style) s.Parent!)?.Type! == StyleValues.Paragraph)
                    .Select(n => ((Style) n.Parent!)).FirstOrDefault();

                if (!forParagraph && style?.LinkedStyle != null)
                {
                    styleId = style.LinkedStyle.Val?.Value;
                }
                else
                {
                    styleId = style?.StyleId;
                }
            }

            if (styleId == null)
                throw new Exception($"Style \"{styleName}\" is not found");
            
            return styleId;
        }
    }
}