using Markdig.Syntax.Inlines;

namespace MD2Word.Markdown.Renderers.InlineRenderers
{
    public class DocCodeInlineRenderer : DocInlineRenderer<CodeInline>
    {
        public DocCodeInlineRenderer(IDocument document) : base(document)
        {
        }

        protected override void Write(DocRenderer renderer, CodeInline obj)
        {
            using var inline = Document.CreateInline();
            inline.SetStyle(FontStyles.CodeText);
            if (obj.Content is { Length: > 0 })
            {
                renderer.Write(obj.ContentWithTrivia);
            }

            using var resetInlineStyle = Document.CreateInline();
        }
    }
}