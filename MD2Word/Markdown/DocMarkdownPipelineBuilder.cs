using Markdig;
using Markdig.Extensions.Tables;
using Markdig.Parsers.Inlines;
using MD2Word.Markdown.Parsers;

namespace MD2Word.Markdown
{
    public class DocMarkdownPipelineBuilder : MarkdownPipelineBuilder
    {
        public DocMarkdownPipelineBuilder()
        {
            BlockParsers.Insert(0, new PipeTableBlockParser());
            BlockParsers.Insert(0, new GridTableParser());
            BlockParsers.Add(new ExtendedBlockParser());
                
            var lineBreakParser = InlineParsers.FindExact<LineBreakInlineParser>();
            InlineParsers.InsertBefore<EmphasisInlineParser>(new PipeTableParser(lineBreakParser!, new PipeTableOptions()));
        }
    }
}