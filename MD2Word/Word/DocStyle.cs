using System;
using System.Collections.Generic;
using System.Reflection;

namespace MD2Word.Word
{
    public class DocStyle
    {
        private readonly IReadOnlyDictionary<FontStyles, string> _styles;

        public DocStyle(IReadOnlyDictionary<FontStyles, string> styles)
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
        public string? Foreground { get; set; }
        public string? Background { get; set; }

        public string this[FontStyles style] => _styles[style];
    }
}