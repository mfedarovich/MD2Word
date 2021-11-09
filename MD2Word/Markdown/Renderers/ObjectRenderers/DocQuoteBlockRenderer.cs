using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace MD2Word.Markdown.Renderers.ObjectRenderers
{
    public class DocQuoteBlockRenderer : DocObjectRenderer<QuoteBlock>
    {
        public DocQuoteBlockRenderer(IDocument document) : base(document)
        {
        }

        protected override void Write(DocRenderer renderer, QuoteBlock quoteBlock)
        {
            using var paragraph = Document.CreateParagraph();
            paragraph.SetStyle(FontStyles.Quote);

            renderer.WriteChildren(quoteBlock);
        }
    }
}