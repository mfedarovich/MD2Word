using System;
using Markdig.Renderers;
using Markdig.Syntax;
using MD2Word.Markdown.Renderers.InlineRenderers;
using MD2Word.Markdown.Renderers.ObjectRenderers;

namespace MD2Word.Markdown.Renderers
{
    public class DocRenderer : TextRendererBase<DocRenderer>
    {
        private readonly IDocument _document;
    
        /// <summary>
        /// Initializes a new instance of the <see cref="DocRenderer"/> class.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="writer">The writer.</param>
        public DocRenderer(IDocument document) : base(document.GetWriter())
        {
            _document = document ?? throw new ArgumentNullException(nameof(document));
            // Default block renderers
            ObjectRenderers.Add(new DocCodeBlockRenderer(document));
            ObjectRenderers.Add(new DocBriefRenderer(document));
            ObjectRenderers.Add(new DocPlantUmlRenderer(document));
            ObjectRenderers.Add(new DocListRenderer(document));
            ObjectRenderers.Add(new DocHeadingStyleRenderer(document));
            ObjectRenderers.Add(new DocHtmlBlockRenderer(document));
            ObjectRenderers.Add(new DocParagraphRenderer(document));
            ObjectRenderers.Add(new DocQuoteBlockRenderer(document));
            ObjectRenderers.Add(new DocThematicBreakRenderer(document));
            //
            // // Default inline renderers
            ObjectRenderers.Add(new DocAutolinkInlineRenderer(document));
            ObjectRenderers.Add(new DocCodeInlineRenderer(document));
            ObjectRenderers.Add(new DocDelimiterInlineRenderer(document));
            ObjectRenderers.Add(new DocEmphasisInlineRenderer(document));
            ObjectRenderers.Add(new DocLineBreakInlineRenderer(document));
            ObjectRenderers.Add(new DocRoundtripHtmlInlineRenderer(document));
            ObjectRenderers.Add(new DocRoundtripHtmlEntityInlineRenderer(document));            
            ObjectRenderers.Add(new DocLinkInlineRenderer(document));
            ObjectRenderers.Add(new DocLiteralInlineRenderer(document));
        }

        public void RenderLinesBefore(Block block)
        {
            if (block.LinesBefore is null)
            {
                return;
            }
            foreach (var line in block.LinesBefore)
            {
                Write(line);
                WriteLine(line.NewLine);
            }
        }

        public void RenderLinesAfter(Block block)
        {
            previousWasLine = true;
            if (block.LinesAfter is null)
            {
                return;
            }
            foreach (var line in block.LinesAfter)
            {
                Write(line);
                WriteLine(line.NewLine);
            }
        }
        public void WriteLeafRawLines(LeafBlock leafBlock)
        {
            if (leafBlock is null) throw  new ArgumentNullException(nameof(leafBlock));
            
            if (leafBlock.Lines.Lines != null)
            {
                var lines = leafBlock.Lines;
                var slices = lines.Lines;
                for (int i = 0; i < lines.Count; i++)
                {
                    var slice = slices[i].Slice;
                    Write(ref slice);
                    WriteLine(slice.NewLine);
                }
            }
        }
    }
}