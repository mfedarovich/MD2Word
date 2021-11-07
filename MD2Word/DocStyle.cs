using System;
using System.Collections.Generic;
using System.Reflection;

namespace MD2Word
{
    public class DocStyle : ICloneable
    {
        private readonly Dictionary<FontStyles, string> _styles;

        public DocStyle(Dictionary<FontStyles, string> styles)
        {
            _styles = styles;
        }

        public FontStyles Style { get; set; } = FontStyles.BodyText;
        public int Level { get; set; } = 0;

        public string Name
        {
            get
            {
                if (Level == 0)
                    return _styles[Style];
                
                var maxLevel = Style.GetType().GetCustomAttribute<NesstingStyleAttribute>()?.MaxLevel ?? int.MaxValue;
                return string.Format(_styles[Style], Math.Min(Level, maxLevel));
            }
        }
        
        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public string this[FontStyles hyperlink] => _styles[hyperlink];
        public object Clone()
        {
            return new DocStyle(_styles) { Bold = Bold, Italic = Italic, Level = Level, Style = Style };
        }
    }
}