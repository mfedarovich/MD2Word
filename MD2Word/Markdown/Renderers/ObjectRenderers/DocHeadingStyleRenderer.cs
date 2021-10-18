﻿using System;
using Markdig.Syntax;

namespace MD2Word.Markdown.Renderers.ObjectRenderers
{
    public class DocHeadingStyleRenderer : DocObjectRenderer<HeadingBlock>
    {
        private const string StylePattern = "Heading {0}";
        private const int MaxHeadingNumber = 4;  
        public DocHeadingStyleRenderer(IDocument document) : base(document)
        {
        }

        protected override void Write(DocRenderer renderer, HeadingBlock obj)
        {
            var style = string.Format(StylePattern, Math.Min(MaxHeadingNumber, obj.Level));
            Document.AddNewBlock(style);
            
            renderer.WriteLeafInline(obj);

            
            renderer.EnsureLine();
        }
    }
}