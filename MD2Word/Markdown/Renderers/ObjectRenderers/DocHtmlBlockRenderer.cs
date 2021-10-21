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
            renderer.RenderLinesBefore(obj);
            //renderer.Write(obj.BeforeWhitespace); // Lines content is written, including whitespace
            renderer.WriteLeafRawLines(obj);
            renderer.RenderLinesAfter(obj);
        }
    }
}