using Markdig.Syntax.Inlines;

namespace MD2Word.Markdown.Renderers.InlineRenderers
{
    public class DocRoundtripHtmlInlineRenderer : DocInlineRenderer<HtmlInline>
    {
        public DocRoundtripHtmlInlineRenderer(IDocument document) : base(document)
        {
        }

        protected override void Write(DocRenderer renderer, HtmlInline obj)
        {
            renderer.Write(obj.Tag);
        }
    }
}