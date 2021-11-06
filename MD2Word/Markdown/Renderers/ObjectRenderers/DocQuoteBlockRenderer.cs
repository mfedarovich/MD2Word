﻿using Markdig.Syntax;
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
            renderer.Write(quoteBlock.TriviaBefore);

            var indents = new string[quoteBlock.QuoteLines.Count];
            for (int i = 0; i < quoteBlock.QuoteLines.Count; i++)
            {
                var quoteLine = quoteBlock.QuoteLines[i];
                var wsb = quoteLine.TriviaBefore.ToString();
                var quoteChar = quoteLine.QuoteChar ? ">" : "";
                var spaceAfterQuoteChar = quoteLine.HasSpaceAfterQuoteChar ? " " : "";
                var wsa = quoteLine.TriviaAfter.ToString();
                indents[i] = (wsb + quoteChar + spaceAfterQuoteChar + wsa);
            }

            if (quoteBlock.Count == 0)
            {
                // since this QuoteBlock instance has no children, indents will not be rendered. We
                // work around this by adding empty LineBreakInlines to a ParagraphBlock.
                // Wanted: a more elegant/better solution (although this is not *that* bad).
                foreach (var quoteLine in quoteBlock.QuoteLines)
                {
                    var emptyLeafBlock = new ParagraphBlock
                    {
                        NewLine = quoteLine.NewLine
                    };
                    var newLine = new LineBreakInline
                    {
                        NewLine = quoteLine.NewLine
                    };
                    var container = new ContainerInline();
                    container.AppendChild(newLine);
                    emptyLeafBlock.Inline = container;
                    quoteBlock.Add(emptyLeafBlock);
                }
            }

            renderer.WriteChildren(quoteBlock);
        }
    }
}