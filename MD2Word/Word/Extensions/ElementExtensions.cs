using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace MD2Word.Word.Extensions
{
    public static class ElementExtensions
    {
        public static bool IsPlaceholder(this OpenXmlElement element)
        {
            return element is SdtElement;
        }
    }
}