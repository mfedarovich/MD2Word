using System;
using System.Collections.Generic;
using System.Reflection;
using MD2Word.Word.Extensions;

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

        public string Name => Level == 0 ? _styles[Style] : string.Format(_styles[Style], Math.Min(Level, Style.GetMaxLevel()));

        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public string? Foreground { get; set; }
        public string? Background { get; set; }

        public string this[FontStyles style] => _styles[style];
    }
}