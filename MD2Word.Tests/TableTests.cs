using System.Linq;
using Markdig.Syntax;
using Markdig.Extensions.Tables;
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
        
        
        [TestCase("| S | T |\r\n|---|---| \r\n| G | H |")]
        [TestCase("| S | \r\n|---|\r\n| G |\r\n\r\n| D | D |\r\n| ---| ---| \r\n| V | V |")]
        public void TablesAreParsedCorrectly2(string markdown)
        {
            TestOutput(markdown, markdown);
        }
    }
}