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
            Document.PushStyle(FontStyles.Heading, obj.Level);
            Document.StartNextParagraph();
            renderer.WriteLeafInline(obj);
            
            Document.PopStyle(false);
        }
    }
}