using System;
using NUnit.Framework;

namespace MD2Word
{
    [TestFixture]
    internal class BaseTestAutoLinkInlineTests : BaseTest
    {
        [TestCase("<http://a>", "http://a")]
        [TestCase(" <http://a>", "http://a")]
        [TestCase("<http://a> ", "http://a")]
        [TestCase(" <http://a> ", "http://a")]
        [TestCase("<example@example.com>","example@example.com")]
        [TestCase(" <example@example.com>","example@example.com")]
        [TestCase("<example@example.com> ","example@example.com")]
        [TestCase(" <example@example.com> ","example@example.com")]
        public void Test(string value, string url)
        {
            TestOutput(value, $"p{Environment.NewLine}h:{url}{Environment.NewLine}");
        }
    }
}