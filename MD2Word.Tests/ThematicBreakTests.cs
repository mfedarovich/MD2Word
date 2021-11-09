using NUnit.Framework;

namespace MD2Word
{
    [TestFixture]
    public class ThematicBreakTests : BaseTest
    {
        [TestCase("***")]
        [TestCase("---")]
        [TestCase("___")]
        public void Test(string markdown)
        {
            TestOutput(markdown, "p\r\n{hz rule}\r\n");
        }
    }
}