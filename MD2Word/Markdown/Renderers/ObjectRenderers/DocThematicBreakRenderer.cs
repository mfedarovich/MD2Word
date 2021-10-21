using Markdig.Syntax;

namespace MD2Word.Markdown.Renderers.ObjectRenderers
{
    public class DocThematicBreakRenderer : DocObjectRenderer<ThematicBreakBlock>
    {
        public DocThematicBreakRenderer(IDocument document) : base(document)
        {
        }

        protected override void Write(DocRenderer renderer, ThematicBreakBlock obj)
        {
            renderer.RenderLinesBefore(obj);

            renderer.Write(obj.Content);
            renderer.WriteLine(obj.NewLine);
            renderer.RenderLinesAfter(obj);
        }
    }
}