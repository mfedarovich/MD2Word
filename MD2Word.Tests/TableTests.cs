using System.Linq;
using FakeItEasy;
using Markdig.Syntax;
using Markdig.Extensions.Tables;
using MD2Word.Markdown.Renderers;
using NUnit.Framework;

namespace MD2Word
{
    [TestFixture]
    public class TableTests : BaseTest
    {
        [TestCase("| S | T |\r\n|---|---| \r\n| G | H |")]
        [TestCase("| S | \r\n|---|\r\n| G |\r\n\r\n| D | D |\r\n| ---| ---| \r\n| V | V |", 2)]
        public void TablesAreParsedCorrectly(string markdown, int tableCount = 1)
        {
            var document = Markdig.Markdown.Parse(markdown, Pipeline);

            var tables = document.Descendants().OfType<Table>().ToArray();

            Assert.AreEqual(tableCount, tables.Length);
        }
        
        [TestCase("| S | T |\r\n|:---|:---| \r\n| G | H |", CellAlignment.Left)]
        [TestCase("| S | T |\r\n|:---:|:---:| \r\n| G | H |", CellAlignment.Center)]
        [TestCase("| S | T |\r\n|---:|---:| \r\n| G | H |", CellAlignment.Right)]
        public void AlignmentTests(string markdown, CellAlignment alignment)
        {
            var table = A.Fake<ITable>();
            var row = A.Fake<IRow>();
            var cell = A.Fake<ICell>();
            var document = A.Fake<IDocument>();
            A.CallTo(() => document.CreateTable()).Returns(table);
            A.CallTo(() => table.AddRow(A<bool>.Ignored)).Returns(row);
            A.CallTo(() => row.AddCell()).Returns(cell);

            Markdig.Markdown.Convert(markdown, new DocRenderer(document), Pipeline);

            A.CallTo(() => cell.Align(A<CellAlignment>.That.IsEqualTo(alignment))).MustHaveHappened();
        }
    }
}