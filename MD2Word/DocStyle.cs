using System;
using System.Collections.Generic;
using System.Reflection;
using DocumentFormat.OpenXml;

namespace MD2Word
{
    public class DocStyle
    {
        private readonly Stack<Tuple<FontStyles, int>> _paragraphStyles = new();
        private readonly Stack<FontStyles> _inlineStyles = new();
        private readonly Dictionary<FontStyles, string> _styles;

        public DocStyle(Dictionary<FontStyles, string> styles)
        {
            _paragraphStyles.Push(new Tuple<FontStyles, int>(FontStyles.BodyText, 0));
            _inlineStyles.Push(FontStyles.BodyText);
            _styles = styles;
        }

        public string ParagraphName
        {
            get
            {
                var style = _paragraphStyles.Peek();
                if (style.Item2 == 0)
                    return _styles[style.Item1];
                
                
                var maxLevel = style.Item1.GetType().GetCustomAttribute<NesstingStyleAttribute>()?.MaxLevel ?? int.MaxValue;
                return string.Format(_styles[style.Item1], Math.Min(style.Item2, maxLevel));
            }
        }

        public string InlineName => _styles[Inline];
        
        public FontStyles Paragraph => _paragraphStyles.Peek().Item1;
        public FontStyles Inline => _inlineStyles.Peek();
        
        public bool Bold { get; set; }
        public bool Italic { get; set; }

        public void Push(FontStyles style, bool inline)
        {
            if (inline)
                _inlineStyles.Push(style);
            else
                _paragraphStyles.Push(new Tuple<FontStyles, int>(style, 0));
        }

        public void Push(FontStyles style, int nestingLevel)
        {
            _paragraphStyles.Push(new Tuple<FontStyles, int>(style, nestingLevel));
        }

        public void Pop(bool inline)
        {
            if (inline)
                _inlineStyles.Pop();
            else
                _paragraphStyles.Pop();
        }

        public string this[FontStyles hyperlink] => _styles[hyperlink];
    }
}