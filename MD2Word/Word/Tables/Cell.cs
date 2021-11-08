using System;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using MD2Word.Word.Extensions;

namespace MD2Word.Word.Tables
{
    public class Cell : ICell
    {
        private readonly TableRow _row;
        private readonly TableCell _cell;

        public Cell(TableRow row, Action<OpenXmlElement> setParent, bool isHeader)
        {
            _row = row;
            _cell = new TableCell();
            if (isHeader)
            {
                _cell.AppendChild(new TableCellProperties()
                {
                    Shading = new Shading
                    {
                        Val = ShadingPatternValues.Clear,
                        Color = StringValue.FromString("auto"),
                        Fill = StringValue.FromString("D9D9D9"),
                        ThemeFill = ThemeColorValues.Background1,
                        ThemeFillShade = StringValue.FromString("D9")
                    },
                    TableCellVerticalAlignment = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }
                });
            }
            _row.AppendChild(_cell);
       
            var paragraph = new Paragraph();
            _cell.AppendChild(paragraph);
            setParent(paragraph);
        }

        public int ColumnSpan { get; set; }
        public int RowSpan { get; set; }
        public void Align(CellAlignment cellAlignment)
        {
            var paragraph = _cell.ChildElements.First<Paragraph>();
            paragraph?.Align(cellAlignment);
        }

        public void Dispose()
        {
        }
    }
}