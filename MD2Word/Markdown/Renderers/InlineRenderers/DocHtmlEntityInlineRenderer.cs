using Markdig.Syntax.Inlines;

namespace MD2Word.Markdown.Renderers.InlineRenderers
{
    public class DocHtmlEntityInlineRenderer : DocInlineRenderer<HtmlEntityInline>
    {
        public DocHtmlEntityInlineRenderer(IDocument document) : base(document)
        {
        }

        protected override void Write(DocRenderer renderer, HtmlEntityInline obj)
        {
            using var inline = Document.CreateInline();
            inline.WriteSymbol(obj.Original.ToString());
        }
    }
}