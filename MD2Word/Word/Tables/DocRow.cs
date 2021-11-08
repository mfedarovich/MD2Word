using System;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using MD2Word.Word.Extensions;

namespace MD2Word.Word.Tables
{
    public class DocRow : IRow
    {
        private readonly Table _table;
        private readonly Action<OpenXmlElement> _setParent;
        private readonly bool _isHeader;
        private readonly TableRow _row;

        public DocRow(Table table, Action<OpenXmlElement> setParent, bool isHeader)
        {
            _table = table;
            _setParent = setParent;
            _isHeader = isHeader;
            _row = new TableRow();
            _table.AppendChild(_row);
            _setParent(_row);
        }

        public ICell AddCell()
        {
            return new Cell(_row, _setParent, _isHeader);
        }

        public void Dispose()
        {
            if (_isHeader)
            {
                foreach (var cell in _row.Descendants<TableCell>())
                {
                    foreach (var run in cell.Descendants<Run>())
                    {
                        run.Emphasise(false, true);
                    }
                }
            }
        }
    }
}