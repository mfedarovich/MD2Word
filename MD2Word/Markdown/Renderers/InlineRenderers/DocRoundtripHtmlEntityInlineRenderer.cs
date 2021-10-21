using Markdig.Syntax.Inlines;

namespace MD2Word.Markdown.Renderers.InlineRenderers
{
    public class DocRoundtripHtmlEntityInlineRenderer : DocInlineRenderer<HtmlEntityInline>
    {
        public DocRoundtripHtmlEntityInlineRenderer(IDocument document) : base(document)
        {
        }

        protected override void Write(DocRenderer renderer, HtmlEntityInline obj)
        {
            renderer.Write(obj.Original);
        }
    }
}