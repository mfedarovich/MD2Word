using Markdig.Syntax.Inlines;

namespace MD2Word.Markdown.Renderers.ObjectRenderers
{
    public class DocLiteralInlineRenderer : DocObjectRenderer<LiteralInline>
    {
        public DocLiteralInlineRenderer(IDocument document) : base(document)
        {
        }

        protected override void Write(DocRenderer renderer, LiteralInline obj)
        {
            renderer.Write(ref obj.Content);
        }
    }
}