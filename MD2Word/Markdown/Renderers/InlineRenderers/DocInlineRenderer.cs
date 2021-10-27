using Markdig.Renderers;
using Markdig.Syntax;

namespace MD2Word.Markdown.Renderers.InlineRenderers
{
    public abstract class DocInlineRenderer<TObject> : MarkdownObjectRenderer<DocRenderer, TObject> where TObject : MarkdownObject
    {
        public IDocument Document { get; }

        public DocInlineRenderer(IDocument document)
        {
            Document = document;
        }
    }
}