using Markdig.Syntax.Inlines;

namespace MD2Word.Markdown.Renderers.InlineRenderers
{
    public class DocHtmlInlineRenderer : DocInlineRenderer<HtmlInline>
    {
        public DocHtmlInlineRenderer(IDocument document) : base(document)
        {
        }

        protected override void Write(DocRenderer renderer, HtmlInline obj)
        {
            using var inline = Document.CreateInline();
            inline.WriteSymbol(obj.Tag);
        }
    }
}