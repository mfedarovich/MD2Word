using System.Linq;
using DocumentFormat.OpenXml.Drawing;
using FakeItEasy;
using Markdig;
using Markdig.Syntax;
using MD2Word.Markdown.Renderers;
using NUnit.Framework;

namespace MD2Word
{
    [TestFixture]
    public class TableTests : BaseTest
    {
        [Test]
        public void TableTest()
        {
            var tableMD = @"| Plugin | README |
| ------ | ------ |
| Dropbox | [plugins/dropbox/README.md][PlDb] |;";
                
                TestOutput(tableMD, tableMD);
        }
        
        [TestCase("| S | T |\r\n|---|---| \r\n| G | H |")]
        [TestCase("| S | T |\r\n|---|---|\t\r\n| G | H |")]
        [TestCase("| S | T |\r\n|---|---|\v\r\n| G | H |")]
        [TestCase("| S | T |\r\n|---|---|\f\r\n| G | H |")]
        [TestCase("| S | T |\r\n|---|---|\f\v\t \r\n| G | H |")]
        [TestCase("| S | \r\n|---|\r\n| G |\r\n\r\n| D | D |\r\n| ---| ---| \r\n| V | V |", 2)]
        public void TestTableBug(string markdown, int tableCount = 1)
        {
            var document = Markdig.Markdown.Parse(markdown, Pipeline);

            Table[] tables = document.Descendants().OfType<Table>().ToArray();

            Assert.AreEqual(tableCount, tables.Length);
        }
    }
}