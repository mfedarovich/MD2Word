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
                $@"p{Environment.NewLine}p{Environment.NewLine}img-{(isLocal ? "file" : "url")}:{url}{Environment.NewLine}";
            TestOutput(value, expected);
        }
        
        [TestCase(" ![text local file](./md2word.dll)", "text local file", "./md2word.dll", true)]
        [TestCase("   ![description](http://example.com)", "description", "http://example.com", false)]
        public void TestLinkWithDescription(string value, string description, string url, bool isLocal)
        {
            var expected =
                $@"p{Environment.NewLine}[{description}]p{Environment.NewLine}img-{(isLocal ? "file" : "url")}:{url}{Environment.NewLine}";
            TestOutput(value, expected);
        }

        [Test]
        public void TestParagraph()
        {
            var value = "paragraph ![description](http://example.com)";
            var expected =
                $@"p{Environment.NewLine}[paragraph ][description]p{Environment.NewLine}img-url:http://example.com{Environment.NewLine}";
            TestOutput(value, expected);
        }
    }
}