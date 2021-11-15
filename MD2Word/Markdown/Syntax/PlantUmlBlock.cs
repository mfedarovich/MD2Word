using Markdig.Parsers;
using Markdig.Syntax;

namespace MD2Word.Markdown.Syntax
{
    public class PlantUmlBlock : LeafBlock
    {
        public PlantUmlBlock(BlockParser? parser) : base(parser)
        {
        }
    }
}