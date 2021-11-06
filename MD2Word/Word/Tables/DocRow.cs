using System;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace MD2Word.Word.Tables
{
    public class DocRow : IRow
    {
        private readonly Table _table;
        private readonly Action<OpenXmlElement> _setParent;
        private readonly TableRow _row;

        public DocRow(Table table, Action<OpenXmlElement> setParent)
        {
            _table = table;
            _setParent = setParent;
            _row = new TableRow();
            _table.AppendChild(_row);
            _setParent(_row);
        }

        public ICell AddCell()
        {
            return new Cell(_row, _setParent);
        }
    }
}