using Markdig.Syntax;

namespace MD2Word.Markdown.Renderers.ObjectRenderers
{
    public class DocHeadingStyleRenderer : DocObjectRenderer<HeadingBlock>
    {
        public DocHeadingStyleRenderer(IDocument document) : base(document)
        {
        }

        protected override void Write(DocRenderer renderer, HeadingBlock obj)
        {
            if (obj.Level == 1)
            {
                using var paragraph = Document.CreateTitle();
                paragraph.SetStyle(FontStyles.Title);
                
                renderer.WriteLeafInline(obj);
            }
            else
            {
                using var paragraph = Document.CreateParagraph();
                paragraph.SetStyle(FontStyles.Heading, obj.Level - 1);
                
                renderer.WriteLeafInline(obj);
            }
        }
    }
}