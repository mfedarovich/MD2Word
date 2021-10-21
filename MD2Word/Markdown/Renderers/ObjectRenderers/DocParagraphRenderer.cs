using System;
using Markdig.Renderers.Roundtrip;
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
            renderer.RenderLinesBefore(paragraph);
            renderer.Write(paragraph.TriviaBefore);
            renderer.WriteLeafInline(paragraph);
            //renderer.Write(paragraph.Newline); // paragraph typically has LineBreakInlines as closing inline nodes
            renderer.RenderLinesAfter(paragraph);
        }


    }
}