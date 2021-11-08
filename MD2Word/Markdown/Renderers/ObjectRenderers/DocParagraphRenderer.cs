using Markdig.Syntax;

namespace MD2Word.Markdown.Renderers.ObjectRenderers
{
    public class DocParagraphRenderer : DocObjectRenderer<ParagraphBlock>
    {
        public DocParagraphRenderer(IDocument document) : base(document)
        {
        }
        protected override void Write(DocRenderer renderer, ParagraphBlock paragraph)
        {
            using var p = Document.CreateParagraph();
            renderer.WriteLeafInline(paragraph);
        }
    }
}