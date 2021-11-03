using Markdig.Extensions.Tables;

namespace MD2Word.Markdown.Renderers.ObjectRenderers
{
    public class DocTableRenderer : DocObjectRenderer<Table>
    {
        public DocTableRenderer(IDocument document) : base(document)
        {
        }

        protected override void Write(DocRenderer renderer, Table obj)
        {
            //         renderer.EnsureLine();
            // renderer.Write("<table").WriteAttributes(table).WriteLine('>');
            //
            // bool hasBody = false;
            // bool hasAlreadyHeader = false;
            // bool isHeaderOpen = false;
            //
            //
            // bool hasColumnWidth = false;
            // foreach (var tableColumnDefinition in table.ColumnDefinitions)
            // {
            //     if (tableColumnDefinition.Width != 0.0f && tableColumnDefinition.Width != 1.0f)
            //     {
            //         hasColumnWidth = true;
            //         break;
            //     }
            // }
            //
            // if (hasColumnWidth)
            // {
            //     foreach (var tableColumnDefinition in table.ColumnDefinitions)
            //     {
            //         var width = Math.Round(tableColumnDefinition.Width*100)/100;
            //         var widthValue = string.Format(CultureInfo.InvariantCulture, "{0:0.##}", width);
            //         renderer.WriteLine($"<col style=\"width:{widthValue}%\" />");
            //     }
            // }
            //
            // foreach (var rowObj in table)
            // {
            //     var row = (TableRow)rowObj;
            //     if (row.IsHeader)
            //     {
            //         // Allow a single thead
            //         if (!hasAlreadyHeader)
            //         {
            //             renderer.WriteLine("<thead>");
            //             isHeaderOpen = true;
            //         }
            //         hasAlreadyHeader = true;
            //     }
            //     else if (!hasBody)
            //     {
            //         if (isHeaderOpen)
            //         {
            //             renderer.WriteLine("</thead>");
            //             isHeaderOpen = false;
            //         }
            //
            //         renderer.WriteLine("<tbody>");
            //         hasBody = true;
            //     }
            //     renderer.Write("<tr").WriteAttributes(row).WriteLine('>');
            //     for (int i = 0; i < row.Count; i++)
            //     {
            //         var cellObj = row[i];
            //         var cell = (TableCell)cellObj;
            //
            //         renderer.EnsureLine();
            //         renderer.Write(row.IsHeader ? "<th" : "<td");
            //         if (cell.ColumnSpan != 1)
            //         {
            //             renderer.Write($" colspan=\"{cell.ColumnSpan}\"");
            //         }
            //         if (cell.RowSpan != 1)
            //         {
            //             renderer.Write($" rowspan=\"{cell.RowSpan}\"");
            //         }
            //         if (table.ColumnDefinitions.Count > 0)
            //         {
            //             var columnIndex = cell.ColumnIndex < 0 || cell.ColumnIndex >= table.ColumnDefinitions.Count
            //                 ? i
            //                 : cell.ColumnIndex;
            //             columnIndex = columnIndex >= table.ColumnDefinitions.Count ? table.ColumnDefinitions.Count - 1 : columnIndex;
            //             var alignment = table.ColumnDefinitions[columnIndex].Alignment;
            //             if (alignment.HasValue)
            //             {
            //                 switch (alignment)
            //                 {
            //                     case TableColumnAlign.Center:
            //                         renderer.Write(" style=\"text-align: center;\"");
            //                         break;
            //                     case TableColumnAlign.Right:
            //                         renderer.Write(" style=\"text-align: right;\"");
            //                         break;
            //                     case TableColumnAlign.Left:
            //                         renderer.Write(" style=\"text-align: left;\"");
            //                         break;
            //                 }
            //             }
            //         }
            //         renderer.WriteAttributes(cell);
            //         renderer.Write('>');
            //
            //         var previousImplicitParagraph = renderer.ImplicitParagraph;
            //         if (cell.Count == 1)
            //         {
            //             renderer.ImplicitParagraph = true;
            //         }
            //         renderer.Write(cell);
            //         renderer.ImplicitParagraph = previousImplicitParagraph;
            //
            //         renderer.WriteLine(row.IsHeader ? "</th>" : "</td>");
            //     }
            //     renderer.WriteLine("</tr>");
            // }
            //
            // if (hasBody)
            // {
            //     renderer.WriteLine("</tbody>");
            // }
            // else if (isHeaderOpen)
            // {
            //     renderer.WriteLine("</thead>");
            // }
            // renderer.WriteLine("</table>");
        }
    }
}