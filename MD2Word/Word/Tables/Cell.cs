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

        public Cell(TableRow row, Action<OpenXmlElement> setParent)
        {
            _row = row;
            _cell = new TableCell();
            _row.AppendChild(_cell);
       
            var paragraph = new Paragraph();
            _cell.AppendChild(paragraph);
            setParent(paragraph);
        }

        public int ColumnSpan { get; set; }
        public int RowSpan { get; set; }
        public void Align(Alignment alignment)
        {
            var paragraph = _cell.ChildElements.First<Paragraph>();
            paragraph?.Align(alignment);
        }
    }
}