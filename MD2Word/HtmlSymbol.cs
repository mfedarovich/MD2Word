using System;
using System.Linq;

namespace MD2Word
{
    public class HtmlSymbol
    {
        public static HtmlSymbol Parse(string htmlSymbol)
        {
            var pos = htmlSymbol.IndexOf("&#", StringComparison.Ordinal);
            if (pos >= 0)
            {
                var startIndex = pos + 2;
                var number = htmlSymbol.Substring(startIndex, htmlSymbol.Length - startIndex - 1);
                if (number.All(c => char.IsNumber(c) || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F')))
                {
                    return new HtmlSymbol(true, number);
                }
            }

            return new HtmlSymbol(false, "");
        }

        protected HtmlSymbol(bool isCode, string value)
        {
            IsCode = isCode;
            Value = value;
        }
        
        public bool IsCode { get; }
        public string Value { get; }
        
    }
}