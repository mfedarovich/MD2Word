using System;
using FakeItEasy;
using MD2Word.Markdown.Renderers;
using NUnit.Framework;

namespace MD2Word
{
    [TestFixture]
    public class EmphasisInlineTests : BaseTest
    {
        [TestCase("_t_")]
        [TestCase("*t*")]
        public void TestEmphasisText(string value)
        {
            TestOutput(value, $"p{Environment.NewLine}[t]");
        }

        [TestCase("_t_")]
        [TestCase("*t*")]
        public void TestEmphasisProperties(string value)
        {
            var document = A.Fake<IDocument>();
            var inline = A.Fake<IInline>();
            A.CallTo(() => document.CreateInline()).Returns(inline);

            Markdig.Markdown.Convert(value, new DocRenderer(document), Pipeline);

            A.CallTo(() => inline.Emphasise(A<bool>.That.IsEqualTo(true), A<bool>.That.IsEqualTo(false))).MustHaveHappened();
        }

        [TestCase("__t__")]
        [TestCase("**t**")]
        [TestCase("****t****")]
        [TestCase("____t____")]
        public void TestStrongEmphasisText(string value)
        {
            TestOutput(value, $"p{Environment.NewLine}[t]");
        }

        [TestCase("__t__")]
        [TestCase("**t**")]
        [TestCase("****t****")]
        [TestCase("____t____")]
        public void TestStrongEmphasisProperties(string value)
        {
            var document = A.Fake<IDocument>();
            var inline = A.Fake<IInline>();
            A.CallTo(() => document.CreateInline()).Returns(inline);

            Markdig.Markdown.Convert(value, new DocRenderer(document), Pipeline);

            A.CallTo(() => inline.Emphasise(A<bool>.That.IsEqualTo(false), A<bool>.That.IsEqualTo(true))).MustHaveHappened();
        }

        [TestCase("___t___")]
        [TestCase("_____t_____")]
        [TestCase("***t***")]
        [TestCase("*****t*****")]
        public void TestBothEmphasisText(string value)
        {
            TestOutput(value, $"p{Environment.NewLine}[t]");
        }
        
        [TestCase("___t___")]
        [TestCase("_____t_____")]
        [TestCase("***t***")]
        [TestCase("*****t*****")]
        public void TestBothEmphasisProperties(string value)
        {
            var document = A.Fake<IDocument>();
            var inline = A.Fake<IInline>();
            A.CallTo(() => document.CreateInline()).Returns(inline);

            Markdig.Markdown.Convert(value, new DocRenderer(document), Pipeline);

            A.CallTo(() => inline.Emphasise(A<bool>.That.IsEqualTo(true), A<bool>.That.IsEqualTo(true))).MustHaveHappened();
        }

    }
}