using System;
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
            using var inline = Document.CreateInline();
            inline.WriteLine();
        }
    }
}