using System;
using System.Linq;
using Markdig.Parsers;
using Markdig.Syntax;
using MD2Word.Markdown.Syntax;

namespace MD2Word.Markdown.Parsers
{ 
    public class ExtendedBlockParser : BlockParser
    {
        private const string Brief = "brief";
        private readonly string[] _planUmlStartArray = {
            "startuml",
            "startgantt",
            "startmindmap",
            "startwbs",
            "startyaml"
        };
        private readonly string[] _plantUmlEndArray = {
            "enduml",
            "endgantt",
            "endmindmap",
            "endwbs",
            "endyaml"
        };

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

            if (TryToCreateBlock<BriefBlock>(processor, new []{Brief}, p => p.IsBlankLine))
            {
                processor.Line.Start += Brief.Length + 1;
                processor.Line.TrimStart();
                return BlockState.Continue;
            }

            if (TryToCreateBlock<PlantUmlBlock>(processor, _planUmlStartArray, p => _plantUmlEndArray.Any(end => p.Line.IndexOf(end)>=0) )) 
                return BlockState.ContinueDiscard;
      

            return BlockState.BreakDiscard;
        }
        public override BlockState TryContinue(BlockProcessor processor, Block block)
        {
            return _checkIfBlockEnded(processor) ? BlockState.BreakDiscard : BlockState.Continue;
        }
        
        private bool TryToCreateBlock<T>(BlockProcessor processor, string[] keys, Func<BlockProcessor, bool> endBlockCheck) 
            where T:LeafBlock
        {
            string key = string.Empty;
            int position = -1;
            foreach (var k in keys)
            {
                position = processor.Line.IndexOf(k, 0, true);
                if (position >= 0)
                {
                    key = k;
                    break;
                }
            }

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