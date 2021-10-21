using Markdig.Parsers;
using Markdig.Syntax;

namespace MD2Word.Markdown.Syntax
{
    public class BriefBlock : LeafBlock
    {
        public BriefBlock(BlockParser? parser) : base(parser)
        {
        }
    }
}