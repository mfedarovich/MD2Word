using System;
using System.Globalization;
using Markdig.Extensions.Tables;

namespace MD2Word.Markdown.Renderers.ObjectRenderers
{
    public class DocTableRenderer : DocObjectRenderer<Table>
    {
        public DocTableRenderer(IDocument document) : base(document)
        {
        }

        protected override void Write(DocRenderer renderer, Table table)
        {
            using var docTable = Document.CreateTable();

            // foreach (var columnDefinition in table.ColumnDefinitions)
            // {
            //     docTable.AddColumnDefinition(columnDefinition.Width);
            // }
            foreach (var rowObj in table)
            {
                var row = (TableRow)rowObj;
                using var docRow = docTable.AddRow(row.IsHeader);
                for (var i = 0; i < row.Count; i++)
                {
                    var cellObj = row[i];
                    var cell = (TableCell)cellObj;

                    using var docCell = docRow.AddCell();
                    docCell.ColumnSpan = cell.ColumnSpan;
                    docCell.RowSpan = cell.RowSpan;
                    
                    if (table.ColumnDefinitions.Count > 0)
                    {
                        var columnIndex = Math.Min(
                                Math.Max(0, cell.ColumnIndex), 
                                table.ColumnDefinitions.Count - 1);
                        
                        var alignment = table.ColumnDefinitions[columnIndex].Alignment;
                        if (alignment.HasValue)
                        {
                            switch (alignment)
                            {
                                case TableColumnAlign.Center:
                                    docCell.Align(CellAlignment.Center);
                                    break;
                                case TableColumnAlign.Right:
                                    docCell.Align(CellAlignment.Right);
                                    break;
                                case TableColumnAlign.Left:
                                    docCell.Align(CellAlignment.Left);
                                    break;
                            }
                        }
                    }
                    renderer.Write(cell);
                }
            }
        }
    }
}