using Markdig.Renderers;
using Markdig.Syntax;

namespace MD2Word.Markdown.Renderers.ObjectRenderers
{
    public abstract class DocObjectRenderer<TObject> : MarkdownObjectRenderer<DocRenderer, TObject> where TObject : MarkdownObject
    {
        protected IDocument Document { get; }

        public DocObjectRenderer(IDocument document)
        {
            Document = document;
        }
    }
}