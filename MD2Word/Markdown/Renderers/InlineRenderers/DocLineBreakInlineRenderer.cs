using System;
using Markdig.Renderers.Roundtrip;
using Markdig.Syntax.Inlines;

namespace MD2Word.Markdown.Renderers.InlineRenderers
{
    public class DocLineBreakInlineRenderer : DocInlineRenderer<LineBreakInline>
    {
        public DocLineBreakInlineRenderer(IDocument document) : base(document)
        {
        }

        protected override void Write(DocRenderer renderer, LineBreakInline obj)
        {
            // if (obj.IsHard && obj.IsBackslash)
            // {
            //     renderer.Write("\\");
            // }
            // renderer.WriteLine(obj.NewLine);
        }
    }
}