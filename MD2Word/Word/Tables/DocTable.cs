using System;
using System.Globalization;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace MD2Word.Word.Tables
{
    public class DocTable : ITable
    {
        private readonly Action<OpenXmlElement> _setParent;
        private readonly Table _table;
        public DocTable(OpenXmlElement beforeElement, Action<OpenXmlElement> setParent)
        {
            _setParent = setParent;
            _table = new Table();
            
            TableProperties props = new();
            props.Append(
                new TableStyle(){Val = "TableGrid"},
                new TableWidth() {Width = "0", Type = TableWidthUnitValues.Auto },
                new TableLook() {Val = "04A0", 
                    FirstRow = OnOffValue.FromBoolean(true), 
                    LastRow = OnOffValue.FromBoolean(false), 
                    FirstColumn = OnOffValue.FromBoolean(true),
                    LastColumn = OnOffValue.FromBoolean(false),
                    NoHorizontalBand = OnOffValue.FromBoolean(false),
                    NoVerticalBand = OnOffValue.FromBoolean(true)
                }
            );
            _table.AppendChild(props);
            
            beforeElement.InsertAfterSelf(_table);
            _setParent(_table);
        }
        
        public IRow AddRow(bool isHeader)
        {
            return new DocRow(_table, _setParent, isHeader);
        }

        public void AddColumnDefinition(float width)
        {
            var grid = _table.Descendants<TableGrid>().FirstOrDefault();
            if (grid == null)
            {
                grid = new TableGrid();
                _table.AppendChild(grid);
            }

            grid.AppendChild(new GridColumn() { Width = StringValue.FromString(Math.Round(width*5000).ToString(CultureInfo.InvariantCulture))  });
        }

        public void Dispose()
        {
            _setParent(_table);
        }

    }
}