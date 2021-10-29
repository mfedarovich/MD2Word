using NUnit.Framework;

namespace MD2Word
{
    [TestFixture]
    public class ImageInlineTests : BaseTest
    {
        [TestCase("![](a)")]
        [TestCase(" ![](a)")]
        [TestCase("![](a) ")]
        [TestCase(" ![](a) ")]
        [TestCase("   ![description](http://example.com)")]
        public void Test(string value)
        {
            TestOutput(value, value);
        }

        [TestCase("paragraph   ![description](http://example.com)")]
        public void TestParagraph(string value)
        {
            TestOutput(value, value);
            
        }
    }
}