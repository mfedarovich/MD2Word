using System;
using System.Collections.Generic;
using System.Linq;

namespace MD2Word
{
    class DocStyle
    {
        private readonly Stack<Tuple<string, bool>> _stack = new();

        public DocStyle()
        {
            _stack.Push(new Tuple<string, bool>("Body Text", false));
        }
        
        public string Style => _stack.Peek().Item1;
        public bool Inline => _stack.Peek().Item2;
        
        public bool Bold { get; set; }
        public bool Italic { get; set; }

        public string ParagraphStyle =>
            Inline ? _stack.AsEnumerable().Reverse().FirstOrDefault(x => !x.Item2)?.Item1 : Style;

        public void Push(string style, bool inline)
        {
            _stack.Push(new Tuple<string, bool>(style, inline));
        }

        public void Pop()
        {
            _stack.Pop();
        }
    }
}