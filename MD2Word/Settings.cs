using System.Collections.Generic;

namespace MD2Word
{
    public class Settings
    {
        public Settings(string drawIoPath, IReadOnlyDictionary<FontStyles, string> styles)
        {
            DrawIoPath = drawIoPath;
            Styles = styles;
        }

        public string DrawIoPath { get; }
        public IReadOnlyDictionary<FontStyles, string> Styles { get; }
    }
}