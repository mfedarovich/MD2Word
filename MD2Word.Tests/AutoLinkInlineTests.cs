using System;
using NUnit.Framework;

namespace MD2Word
{
    [TestFixture]
    internal class AutoLinkInlineTests : BaseTest
    {
        [TestCase("<http://a>", "http://a")]
        [TestCase(" <http://a>", "http://a")]
        [TestCase("<http://a> ", "http://a")]
        [TestCase(" <http://a> ", "http://a")]
        [TestCase("<example@example.com>","example@example.com")]
        [TestCase(" <example@example.com>","example@example.com")]
        [TestCase("<example@example.com> ","example@example.com")]
        [TestCase(" <example@example.com> ","example@example.com")]
        [TestCase("<http://www.google.com>", "http://www.google.com")]
        public void SingleLineConversion(string value, string url)
        {
            TestOutput(value, $"p{Environment.NewLine}h:{url}");
        }

        [Test]
        public void MultiLineConversion()
        {
            var md = @"p1 <http://www.google.com>
p2 <http://www.google.com>";

            var expected = @"p
[p1 ]h:http://www.google.com
[p2 ]h:http://www.google.com";
            TestOutput(md, expected);
        }

    }
}