using System;
using NUnit.Framework;

namespace MD2Word
{
    [TestFixture]
    public class ImageInlineTests : BaseTest
    {
        [TestCase("![](a)", "", "a", false)]
        public void TestEmptyDescription(string value, string description, string url, bool isLocal)
        {
            var expected =
                $"p\r\np\r\nimg-{(isLocal ? "file" : "url")}:{url}\r\n";
            TestOutput(value, expected);
        }
        
        [TestCase(" ![text local file](./md2word.dll)", "text local file", "./md2word.dll", true)]
        [TestCase("   ![description](http://example.com)", "description", "http://example.com", false)]
        public void TestLinkWithDescription(string value, string description, string url, bool isLocal)
        {
            var expected =
                $"p\r\n{{iCAPTION}}[{description}]{{!}}p\r\nimg-{(isLocal ? "file" : "url")}:{url}\r\n";
            TestOutput(value, expected);
        }

        [Test]
        public void TestParagraph()
        {
            var value = "paragraph ![description](http://example.com)";
            var expected =
                $"p\r\n[paragraph ]{{iCAPTION}}[description]{{!}}p\r\nimg-url:http://example.com\r\n";
            TestOutput(value, expected);
        }
    }
}