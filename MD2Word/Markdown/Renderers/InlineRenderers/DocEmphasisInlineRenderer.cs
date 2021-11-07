using Markdig.Syntax.Inlines;

namespace MD2Word.Markdown.Renderers.InlineRenderers
{
    public class DocEmphasisInlineRenderer: DocInlineRenderer<EmphasisInline>
    {
        private int _italicCount = 0;
        private int _boldCount = 0;
        public DocEmphasisInlineRenderer(IDocument document) : base(document)
        {
        }

        protected override void Write(DocRenderer renderer, EmphasisInline obj)
        {
            switch (obj.DelimiterCount)
            {
                case 1: _italicCount++;  break;
                case 2: _boldCount++; break;
            }

            using var inline = Document.CreateInline();
            inline.Emphasise(_italicCount > 0, _boldCount > 0);

            renderer.WriteChildren(obj);

            switch (obj.DelimiterCount)
            {
                case 1: _italicCount--;  break;
                case 2: _boldCount--; break;
            }
            inline.Emphasise(_italicCount > 0, _boldCount > 0);
        }
    }
}