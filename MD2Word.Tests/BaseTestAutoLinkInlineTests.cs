using NUnit.Framework;

namespace MD2Word
{
    [TestFixture]
    internal class BaseTestAutoLinkInlineTests : BaseTest
    {
        [TestCase("<http://a>")]
        [TestCase(" <http://a>")]
        [TestCase("<http://a> ")]
        [TestCase(" <http://a> ")]
        [TestCase("<example@example.com>")]
        [TestCase(" <example@example.com>")]
        [TestCase("<example@example.com> ")]
        [TestCase(" <example@example.com> ")]
        [TestCase("p http://a p")]
        public void Test(string value)
        {
            Check(value, "");
        }
    }
}