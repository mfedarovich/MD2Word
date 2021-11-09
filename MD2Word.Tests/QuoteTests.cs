using NUnit.Framework;

namespace MD2Word
{
    [TestFixture]
    public class QuoteTests : BaseTest
    {
        [TestCase("> 1st line\r\n> 2nd line", "1st line", "2nd line")]
        public void Test(string markdown, string line1, string line2)
        {
            var expected = $"p\r\n{{QUOTE}}p\r\n[{line1}]\r\n[{line2}]{{!}}";
            TestOutput(markdown, expected);
        }
        
    }
}