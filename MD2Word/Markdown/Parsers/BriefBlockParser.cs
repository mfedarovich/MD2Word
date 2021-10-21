using System;
using System.Collections.Generic;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using MD2Word.Markdown.Syntax;

namespace MD2Word.Markdown.Parsers
{ 
    public class BriefBlockParser : BlockParser
    {
        private const string Brief = "brief";

        public BriefBlockParser()
        {
            OpeningCharacters = new[] {'@'};
        }
        
        public override BlockState TryOpen(BlockProcessor processor)
        {
            if (processor.IsCodeIndent)
            {
                return BlockState.None;
            }

            var position = processor.Line.IndexOf(Brief,0, true);

            if (position > 0)
            {
                processor.Column = position + Brief.Length;
                processor.NewBlocks.Push(new BriefBlock(this));
                return BlockState.Continue;
            }
            
            
            return BlockState.BreakDiscard;
        }

        public override BlockState TryContinue(BlockProcessor processor, Block block)
        {
            if (processor.IsBlankLine)
            {
                return BlockState.BreakDiscard;
            }
            
            block.NewLine = processor.Line.NewLine;
            block.UpdateSpanEnd(processor.Line.End);
            (block as BriefBlock)?.AppendLine(ref processor.Line, processor.Column, processor.LineIndex,
                processor.CurrentLineStartPosition, processor.TrackTrivia);
            
            return BlockState.Continue;
        }
    }
}