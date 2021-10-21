using Markdig.Parsers;
using Markdig.Syntax;
using MD2Word.Markdown.Syntax;

namespace MD2Word.Markdown.Parsers
{
    public class PlantUmlParser : BlockParser
    {
        public PlantUmlParser()
        {
            OpeningCharacters = new[] {'@'};
        }

        public override BlockState TryOpen(BlockProcessor processor)
        {
            if (processor.IsCodeIndent)
            {
                return BlockState.None;
            }

            var position = processor.Line.IndexOf("startuml", 0, true);

            if (position > 0)
            {
                var block = new PlantUmlBlock(this);
                var codeBlockLine = new PlantUmlBlock.UmlBlockLine()
                {
                    TriviaBefore = processor.UseTrivia(processor.Start - 1)
                };
                block.BlockLines.Add(codeBlockLine);

                processor.NewBlocks.Push(block);
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

            var position = processor.Line.IndexOf("enduml", 0, true);
            if (position > 0)
            {
                return BlockState.ContinueDiscard;
            }

            var cb = (PlantUmlBlock) block;
            var codeBlockLine = new PlantUmlBlock.UmlBlockLine
            {
                TriviaBefore = processor.UseTrivia(processor.Start - 1)
            };
            cb.BlockLines.Add(codeBlockLine);
            cb.NewLine = processor.Line.NewLine; // ensure block newline is last newline

            return BlockState.Continue;
        }
    }
}