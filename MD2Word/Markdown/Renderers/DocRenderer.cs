using System;
using System.Runtime.CompilerServices;
using Markdig.Helpers;
using Markdig.Renderers;
using Markdig.Syntax;
using MD2Word.Markdown.Renderers.InlineRenderers;
using MD2Word.Markdown.Renderers.ObjectRenderers;
using Inline = Markdig.Syntax.Inlines.Inline;

namespace MD2Word.Markdown.Renderers
{
    public class DocRenderer : RendererBase
    {
        private readonly IDocument _document;
#if !NETCORE
        private char[] _buffer = new char[1024];
#endif
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DocRenderer"/> class.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="writer">The writer.</param>
        public DocRenderer(IDocument document)
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
            ObjectRenderers.Add(new DocHtmlInlineRenderer(document));
            ObjectRenderers.Add(new DocHtmlEntityInlineRenderer(document));            
            ObjectRenderers.Add(new DocLinkInlineRenderer(document));
            ObjectRenderers.Add(new DocLiteralInlineRenderer(document));
            
            ObjectRenderers.Add(new DocTableRenderer(document));
            
        }
        
        public override object Render(MarkdownObject markdownObject)
        {
            Write(markdownObject);
            return _document;
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
                }
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(string? content)
        {
            _document.Writer.WriteText(content!);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(StringSlice slice)
        {
            Write(ref slice);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(ref StringSlice slice)
        {
            if (slice.Start > slice.End)
            {
                return;
            }
            Write(slice.Text, slice.Start, slice.Length);
        }
        
        public void Write(string content, int offset, int length, bool inline = false)
        {
            if (content is null)
            {
                return;
            }
            
#if NETCORE
            Writer.Write(content.AsSpan(offset, length));
#else
            if (offset != 0 || content.Length != length)
            {
                if (length > _buffer.Length)
                {
                    _buffer = content.ToCharArray();
                }
                else
                {
                    content.CopyTo(offset, _buffer, 0, length);
                    offset = 0;
                }
                
                content = new string(_buffer, offset, length);
            }

            _document.Writer.WriteText(content);
#endif
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteLeafInline(LeafBlock leafBlock)
        {
            if (leafBlock is null) throw new ArgumentNullException(nameof(leafBlock));
            var inline = (Inline) leafBlock.Inline!;
          
            while (inline != null)
            {
                Write(inline);
                inline = inline.NextSibling;
            }
        }
    }
}