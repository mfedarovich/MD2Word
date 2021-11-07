using System;
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
            using var paragraph = Document.CreateParagraph();
            paragraph.SetStyle(FontStyles.Heading, obj.Level);
            renderer.WriteLeafInline(obj);
        }
    }
}