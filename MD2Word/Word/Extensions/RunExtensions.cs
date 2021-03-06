using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace MD2Word.Word.Extensions
{
    public static class RunExtensions
    {
        public static Run ApplyStyle(this Run run, WordprocessingDocument doc, DocStyle style)
        {
            run.ApplyStyleId(doc.FindStyleIdByName(style.Name, false));
            run.Emphasise(style.Italic, style.Bold);
            run.SetColors(style.Foreground, style.Background);

            return run;
        }
        public static Run AppendText(this Run run, string text, bool preserveSpace = false)
        {
            var textBlock = new Text(text);
            if (preserveSpace)
                textBlock.Space = new EnumValue<SpaceProcessingModeValues>(SpaceProcessingModeValues.Preserve);
            run.AppendChild(textBlock);
            return run;
        }
        public static Run AppendSymbol(this Run run, HtmlSymbol symbol)
        {
            OpenXmlElement child;
            if (symbol.IsCode)
            {
                child = new SymbolChar()
                {
                    // -1 - ignore ';' at the end
                    Char = new HexBinaryValue() { Value = symbol.Value },
                    Font = new StringValue() { Value = "Symbol" }
                };
                run.AppendChild(child);
            }
            else
            {
                run.AppendText(symbol.Value);
            }
            return run;
        }
        public static Run Emphasise(this Run run, bool italic, bool bold)
        {
            if(!italic && !bold)
                return run;
            
            var pPr = run.Elements<RunProperties>().FirstOrDefault() ??
                      run.PrependChild(new RunProperties());

            if (bold)
            {
                Bold boldStyle = new()
                {
                    Val = OnOffValue.FromBoolean(true)
                };
                pPr.AppendChild(boldStyle);
            }

            if (italic)
            {
                Italic italicStyle = new()
                {
                    Val = OnOffValue.FromBoolean(true)
                };
                pPr.AppendChild(italicStyle);
            }

            return run;
        }

        public static Run SetColors(this Run run, string? foreground, string? background)
        {
            if(string.IsNullOrEmpty(foreground) && string.IsNullOrEmpty(background))
                return run;
            
            var pPr = run.Elements<RunProperties>().FirstOrDefault() ??
                      run.PrependChild(new RunProperties());

            if (foreground is not null)
            {
                pPr.Color = new Color() { Val = foreground };
            }

            if (background is not null)
            {
                pPr.Color ??= new Color();
                pPr.Color.ThemeShade = background;
            }

            return run;        
        }
        public static void ApplyStyleId(this Run run, string styleId)
        {
            var pPr = run.Elements<RunProperties>().FirstOrDefault() ??
                      run.PrependChild(new RunProperties());
            pPr.RunStyle ??= new RunStyle();
            pPr.RunStyle.Val = styleId;
        }
    }
}