using System;
using Markdig.Syntax.Inlines;

namespace MD2Word.Markdown.Renderers.InlineRenderers
{
    public class DocDelimiterInlineRenderer : DocInlineRenderer<DelimiterInline>
    {
        
        public DocDelimiterInlineRenderer(IDocument document) : base(document)
        {
        }

        protected override void Write(DocRenderer renderer, DelimiterInline obj)
        {
            renderer.Write((string?) obj.ToLiteral());
            renderer.WriteChildren(obj);
        }

    }
}