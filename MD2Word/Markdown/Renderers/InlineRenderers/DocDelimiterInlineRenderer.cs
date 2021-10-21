using System;
using Markdig.Renderers.Roundtrip;
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
            Document.AddNewBlock("InfoBlue");
            Document.WriteText("<!-DelimiterInline-->" + Environment.NewLine);
            renderer.Write(obj.ToLiteral());
            renderer.WriteChildren(obj);
        }

    }
}