using Markdig.Renderers;
using Markdig.Syntax;

namespace MD2Word.Markdown.Renderers.ObjectRenderers
{
    public abstract class DocObjectRenderer<TObject> : IMarkdownObjectRenderer 
        where TObject : MarkdownObject
    {
        protected IDocument Document { get; }

        protected DocObjectRenderer(IDocument document)
        {
            Document = document;
        }

        public bool Accept(RendererBase renderer, MarkdownObject obj)
        {
            return obj is TObject;
        }

        public void Write(RendererBase renderer, MarkdownObject objectToRender)
        {
            Write((DocRenderer)renderer, (TObject)objectToRender);
        }
        
        protected abstract void Write(DocRenderer renderer, TObject obj);

    }
}