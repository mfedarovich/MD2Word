using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace MD2Word.Word
{
    public static class RunExtensions
    {
        public static void Emphasise(this Run run, bool italic, bool bold)
        {
            if(!italic && !bold)
                return;
            
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
        }
        public static void ApplyStyleId(this Run run, string styleId)
        {
            var pPr = run.Elements<RunProperties>().FirstOrDefault() ??
                      run.PrependChild(new RunProperties());
            pPr.RunStyle = new RunStyle { Val = styleId };
        }
    }
}