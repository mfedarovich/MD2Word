using System;
using System.Collections.Generic;
using System.Reflection;

namespace MD2Word
{
    public class DocStyle
    {
        private readonly Stack<Tuple<FontStyles, int>> _paragraphStyles = new();
        private readonly Stack<FontStyles> _inlineStyles = new();
        private readonly Dictionary<FontStyles, string> _converter = new();

        public DocStyle()
        {
            _paragraphStyles.Push(new Tuple<FontStyles, int>(FontStyles.BodyText, 0));
            _inlineStyles.Push(FontStyles.BodyText);
            
            // TODO:
            _converter.Add(FontStyles.BodyText, "Body Text");
            _converter.Add(FontStyles.CodeText, "Code Text");
            _converter.Add(FontStyles.Caption, "Caption_style");
            _converter.Add(FontStyles.CodeBlock, "Code");
            _converter.Add(FontStyles.Heading, "Heading {0}");
            _converter.Add(FontStyles.NumberList, "List Number");
            _converter.Add(FontStyles.BulletList, "List Bullet");
        }

        public string ParagraphName
        {
            get
            {
                var style = _paragraphStyles.Peek();
                if (style.Item2 == 0)
                    return _converter[style.Item1];
                
                
                var maxLevel = style.Item1.GetType().GetCustomAttribute<NesstingStyleAttribute>()?.MaxLevel ?? int.MaxValue;
                return string.Format(_converter[style.Item1], Math.Min(style.Item2, maxLevel));
            }
        }

        public string InlineName => _converter[Inline];
        
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
    }
}