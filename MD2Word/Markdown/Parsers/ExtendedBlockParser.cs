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

        private Func<BlockProcessor, bool> _checkIfBlockEnded = processor => false;
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
            {
                processor.Line.Start += Brief.Length + 1;
                processor.Line.TrimStart();
                return BlockState.Continue;
            }

            if (TryToCreateBlock<PlantUmlBlock>(processor, PlanUmlStart, p => p.Line.IndexOf(PlantUmlEnd)>=0)) 
                return BlockState.ContinueDiscard;
      

            return BlockState.BreakDiscard;
        }
        public override BlockState TryContinue(BlockProcessor processor, Block block)
        {
            return _checkIfBlockEnded(processor) ? BlockState.BreakDiscard : BlockState.Continue;
        }
        
        private bool TryToCreateBlock<T>(BlockProcessor processor, string key, Func<BlockProcessor, bool> endBlockCheck) 
            where T:LeafBlock
        {
            var position = processor.Line.IndexOf(key, 0, true);

            if (position >= 0)
            {
                processor.Column = position + key.Length;
                processor.NewBlocks.Push((T)Activator.CreateInstance(typeof(T), this)!);
                _checkIfBlockEnded = endBlockCheck;
                return true;
            }

            return false;
        }

    }
}