using FakeItEasy;
using MD2Word.Markdown.Renderers;
using NUnit.Framework;

namespace MD2Word
{
    [TestFixture]
    public class InlineLinkTests : BaseTest
    {
        [TestCase("[](url)", "url")]
        public void Test(string value, string url)
        {
            TestOutput(value, $"p\r\nh:{url}");
        }

        [TestCase("[description](url)", "description", "url")]
        public void Test(string value, string description, string url)
        {
            TestOutput(value, $"p\r\n[{description}]h:{url}");
        }

        [TestCase("[a](https://www.google.com/ \"title\") ", "a", "https://www.google.com/", "title")]
        [TestCase("[a](https://www.google.com/ 'title') ", "a", "https://www.google.com/", "title")]
        [TestCase("[a](https://www.google.com/ (title)) ", "a", "https://www.google.com/", "title")]
        [TestCase("[*a*](https://www.google.com/ (title))", "a", "https://www.google.com/", "title")]
        public void Test_Title(string value, string description, string url, string title)
        {
            var expected = $"p\r\n[{description}]h:{url}";
            TestOutput(value, expected);
        }

        [TestCase("[a](<b>)", "a", "b")]
        [TestCase("[a](<b b>)", "a", "b b")]
        public void Test_PointyBrackets(string value, string description, string url)
        {
            var expected = $"p\r\n[{description}]h:{url}";
            TestOutput(value, expected);
        }
        
        [TestCase("[a](https://www.google.com/)", "a", "https://www.google.com/")]
        public void Test_EmptyTitle(string value, string description, string url)
        {
            var expected = $"p\r\n[{description}]h:{url}";
            TestOutput(value, expected);
        }

        [TestCase("[a]()", "a")]
        [TestCase("[a](<>)", "a")]
        [TestCase("[description]()", "description")] 
        [TestCase("[a](< >)", "a")]
        public void Test_EmptyUrl(string value, string description)
        {
            var expected = $"p\r\n[{description}]";
            TestOutput(value, expected);
        }

        [TestCase("[*description*][url (title)]")]
        public void Test_Emphasise(string value)
        {
            var document = A.Fake<IDocument>();
            var inline = A.Fake<IInline>();
            A.CallTo(() => document.CreateInline()).Returns(inline);

            Markdig.Markdown.Convert(value, new DocRenderer(document), Pipeline);

            A.CallTo(() => inline.Emphasise(A<bool>.That.IsEqualTo(true), A<bool>.That.IsEqualTo(false))).MustHaveHappened();
        }

    }
}