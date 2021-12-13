using System;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MD2Word.Word.Extensions;

namespace MD2Word.Word.Blocks
{
    public class DocList
    {
        private readonly NumberingDefinitionsPart _numberingPart;
        private int? _listStyleId;
        private const int IndStart = 360;
        private const int IndStep = 360;

        public DocList(WordprocessingDocument document)
        {
            var numberingPart = document.MainDocumentPart?.NumberingDefinitionsPart;
            if (numberingPart == null)
            {
                numberingPart =
                    document.MainDocumentPart?.AddNewPart<NumberingDefinitionsPart>("NumberingDefinitionsPart001");
                Numbering element = new();
                element.Save(_numberingPart!);
            }
            _numberingPart = numberingPart!;
        }

        public void ApplyStyle(Paragraph paragraph, FontStyles style, int level)
        {
            paragraph.ApplyStyleId("ListParagraph");
            var listId = GetListStyleId(style);
            paragraph.AppendChild(new NumberingProperties()
            {
                NumberingLevelReference = new NumberingLevelReference() { Val = level },
                NumberingId = new NumberingId() { Val = listId }
            });
        }

        private int CreateAbstractNum(FontStyles style)
        {
            var abstractNumberId = _numberingPart.Numbering.Elements<AbstractNum>().Count() + 1;
            AbstractNum abstractNum = new()
            {
                AbstractNumberId = abstractNumberId,
                MultiLevelType = new MultiLevelType() {Val = MultiLevelValues.Multilevel}
            };
            switch (style)
            {
                case FontStyles.NumberList:
                    AddNumberListLevels(abstractNum);
                    break;
                case FontStyles.BulletList:
                    AddBulletListLevels(abstractNum);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(style), style, null);
            }

            if (abstractNumberId == 1)
            {
                _numberingPart.Numbering.AppendChild(abstractNum);
            }
            else
            {
                AbstractNum lastAbstractNum = _numberingPart.Numbering.Elements<AbstractNum>().Last();
                _numberingPart.Numbering.InsertAfter(abstractNum, lastAbstractNum);
            }

            return abstractNumberId;
        }

        private static string GetNumberingTextTemplate(int level)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < level; i++)
            {
                sb.Append($"%{i + 1}.");
            }

            return sb.ToString();
        }

        private static void AddNumberListLevels(AbstractNum abstractNum)
        {
            var maxLevel = Math.Min(10, FontStyles.NumberList.GetMaxLevel());
            for (int i = 0, indStart = IndStart; i < maxLevel; i++, indStart += IndStep)
            {
                var level = new Level()
                {
                    LevelIndex = i,
                    NumberingFormat = new NumberingFormat { Val = NumberFormatValues.Decimal },
                    StartNumberingValue = new StartNumberingValue {Val = 1},
                    LevelText = new LevelText { Val = GetNumberingTextTemplate(i + 1) },
                    LevelJustification = new LevelJustification() {Val = LevelJustificationValues.Left},
                    PreviousParagraphProperties = new PreviousParagraphProperties() {Indentation = new Indentation() {Left = indStart.ToString(), Hanging = IndStart.ToString()}}
                };
                abstractNum.AppendChild(level);        
            }
        }

        private static void AddBulletListLevels(AbstractNum abstractNum)
        {
            var maxLevel = Math.Min(10, FontStyles.BulletList.GetMaxLevel());
            for (int i = 0, indStart = IndStart; i < maxLevel; i++, indStart += IndStep)
            {
                var level = new Level()
                {
                    LevelIndex = i,
                    NumberingFormat = new NumberingFormat { Val = NumberFormatValues.Bullet },
                    LevelText = new LevelText { Val = "" },
                    LevelJustification = new LevelJustification() {Val = LevelJustificationValues.Left},
                    PreviousParagraphProperties = new PreviousParagraphProperties()
                    {
                        Indentation = new Indentation() {Left = indStart.ToString(), Hanging = IndStart.ToString()}
                    },
                    NumberingSymbolRunProperties = new NumberingSymbolRunProperties()
                    {
                        RunFonts = new RunFonts() {Ascii = "Symbol", HighAnsi = "Symbol", Hint = FontTypeHintValues.Default}
                    }
                };
                abstractNum.AppendChild(level);        
            }
        }
        private int CreateNumberingInstance(int abstractNumberId)
        {
            var numberId = _numberingPart.Numbering.Elements<NumberingInstance>().Count() + 1;
            NumberingInstance numberingInstance1 = new() { NumberID = numberId };
            AbstractNumId abstractNumId1 = new() { Val = abstractNumberId };
            numberingInstance1.AppendChild(abstractNumId1);

            if (numberId == 1)
            {
                _numberingPart.Numbering.AppendChild(numberingInstance1);
            }
            else
            {
                var lastNumberingInstance = _numberingPart.Numbering.Elements<NumberingInstance>().Last();
                _numberingPart.Numbering.InsertAfter(numberingInstance1, lastNumberingInstance);
            }

            return numberId;
        }
        
        private int GetListStyleId(FontStyles fontStyle)
        {
            if (_listStyleId is not null)
                return _listStyleId.Value;
            
            var abstractNumberId  = CreateAbstractNum(fontStyle);
            _listStyleId = CreateNumberingInstance(abstractNumberId);
            return _listStyleId.Value;
        }
    }
}