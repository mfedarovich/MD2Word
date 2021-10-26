using System;
using Markdig.Parsers;
using Markdig.Syntax;
using MD2Word.Markdown.Syntax;

namespace MD2Word.Markdown.Parsers
{ 
    public class ExtendedBlockParser : BlockParser
    {
        private const string Brief = "brief";
        private const string PlanUmlStart = "startuml";
        private const string PlantUmlEnd = "enduml";

        private Func<BlockProcessor, bool> CheckIfBlockEnded;
        public ExtendedBlockParser()
        {
            OpeningCharacters = new[] {'@'};
        }
        
        public override BlockState TryOpen(BlockProcessor processor)
        {
            if (processor.IsCodeIndent)
            {
                return BlockState.None;
            }

            if (TryToCreateBlock<BriefBlock>(processor, Brief, p => p.IsBlankLine)) 
                return BlockState.Continue;
            if (TryToCreateBlock<PlantUmlBlock>(processor, PlanUmlStart, p => p.Line.IndexOf(PlantUmlEnd)>=0)) 
                return BlockState.ContinueDiscard;
      

            return BlockState.BreakDiscard;
        }
        public override BlockState TryContinue(BlockProcessor processor, Block block)
        {
            if (CheckIfBlockEnded(processor)) 
                return BlockState.BreakDiscard;
            
            block.NewLine = processor.Line.NewLine;
            block.UpdateSpanEnd(processor.Line.End);
            (block as LeafBlock)?.AppendLine(ref processor.Line, processor.Column, processor.LineIndex,
                processor.CurrentLineStartPosition, processor.TrackTrivia);
            
            return BlockState.Continue;
        }
        
        private bool TryToCreateBlock<T>(BlockProcessor processor, string key, Func<BlockProcessor, bool> endBlockCheck) 
            where T:LeafBlock
        {
            var position = processor.Line.IndexOf(key, 0, true);

            if (position >= 0)
            {
                processor.Column = position + key.Length;
                processor.NewBlocks.Push((T)Activator.CreateInstance(typeof(T), this)!);
                CheckIfBlockEnded = endBlockCheck;
                return true;
            }

            return false;
        }

    }
}