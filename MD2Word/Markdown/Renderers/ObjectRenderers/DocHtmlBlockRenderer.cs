using Markdig.Syntax;

namespace MD2Word.Markdown.Renderers.ObjectRenderers
{
    public class DocHtmlBlockRenderer : DocObjectRenderer<HtmlBlock>
    {
        public DocHtmlBlockRenderer(IDocument document) : base(document)
        {
        }

        protected override void Write(DocRenderer renderer, HtmlBlock obj)
        {
            renderer.WriteLeafRawLines(obj);
        }
    }
}