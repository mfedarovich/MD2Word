using System;
using NUnit.Framework;

namespace MD2Word
{
    [TestFixture]
    public class CodeInlineTests : BaseTest
    {
        [TestCase("``", "")]
        [TestCase(" ``", "")]
        [TestCase("`` ", "")]
        [TestCase(" `` ", "")]
        public void TestEmptyCode(string markdown, string code)
        {
            var expected = $"p{Environment.NewLine}[``]";
            TestOutput(markdown, expected);
        }

        [TestCase("`c`", "c")]
        [TestCase(" `c`", "c")]
        [TestCase("`c` ", "c")]
        [TestCase(" `c` ", "c")]

        [TestCase("` c`", " c")]
        [TestCase(" ` c`", " c")]
        [TestCase("` c` ", " c")]
        [TestCase(" ` c` ", " c")]

        [TestCase("`c `", "c ")]
        [TestCase(" `c `", "c ")]
        [TestCase("`c ` ", "c ")]
        [TestCase(" `c ` ", "c ")]

        [TestCase("``c``", "c")] // 2, 2
        [TestCase("```c```", "c")] // 3, 3
        [TestCase("````c````", "c")] // 4, 4
        
        [TestCase("` a `", " a ")]
        [TestCase(" ` a `", " a ")]
        [TestCase("` a ` ", " a ")]
        [TestCase(" ` a ` ", " a ")]
        public void TestInlineCode(string markdown, string code)
        {
            var expected = @$"p
{{iCODE TEXT}}[{code}]{{!}}";

            TestOutput(markdown, expected);
        }

        [TestCase("p `a` p", "a")]
        [TestCase("p ``a`` p", "a")]
        [TestCase("p ```a``` p", "a")]
        public void TestParagraph(string markdown, string code)
        {
            var expected = @$"p
[p ]{{iCODE TEXT}}[{code}]{{!}}[ p]";
            TestOutput(markdown, expected);
        }

        [TestCase("`\na\n`")]
        [TestCase("`\na\r`")]
        [TestCase("`\na\r\n`")]
        [TestCase("`\ra\r`")]
        [TestCase("`\ra\n`")]
        [TestCase("`\ra\r\n`")]
        [TestCase("`\r\na\n`")]
        [TestCase("`\r\na\r`")]
        [TestCase("`\r\na\r\n`")]
        public void TestNewlines(string markdown)
        {
            var expected = markdown.Substring(1, markdown.Length - 2);
            expected = $"p{Environment.NewLine}{{iCODE TEXT}}[{expected}]{{!}}";
            TestOutput(markdown, expected);
        }

    }
}