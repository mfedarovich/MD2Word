using System;
using NUnit.Framework;

namespace MD2Word
{
    [TestFixture]
    public class HtmlInlineTest : BaseTest
    {
        [TestCase("&gt;")]
        [TestCase("&lt;")]
        [TestCase("&nbsp;")]
        [TestCase("&heartsuit;")]
        [TestCase("&#42;")]
        [TestCase("&#0;")]
        [TestCase("&#1234;")]
        [TestCase("&#xcab;")]

        [TestCase(" &gt; ")]
        [TestCase(" &lt; ")]
        [TestCase(" &nbsp; ")]
        [TestCase(" &heartsuit; ")]
        [TestCase(" &#42; ")]
        [TestCase(" &#0; ")]
        [TestCase(" &#1234; ")]
        [TestCase(" &#xcab; ")]
        public void Test(string value)
        {
            TestOutput(value, $"p{Environment.NewLine}[{value.Trim()}]");
        }
    }
}