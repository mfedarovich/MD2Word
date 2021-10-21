using System.Collections.Generic;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;

namespace MD2Word.Markdown.Syntax
{
    public class PlantUmlBlock : LeafBlock
    {
        public class UmlBlockLine
        {
            public StringSlice TriviaBefore { get; set; }
        }
        
        public PlantUmlBlock(BlockParser? parser) : base(parser)
        {
        }
        
        public List<UmlBlockLine> BlockLines { get; } = new List<UmlBlockLine>();

    }
}