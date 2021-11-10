using Markdig;
using MD2Word.Markdown;
using MD2Word.Markdown.Renderers;
using MD2Word.Stubs;
using NUnit.Framework;

namespace MD2Word
{
    public class BaseTest
    {
        private DocumentStub? Document { get; set; } 
        private DocRenderer? Renderer { get; set; }
        protected MarkdownPipeline? Pipeline { get; private set; }
        
        [SetUp]
        public void Setup()
        {
            Document = new DocumentStub();
            Renderer = new DocRenderer(Document);
            
            var pipelineBuilder = new DocMarkdownPipelineBuilder();
            Pipeline = pipelineBuilder.Build();
        }

        protected void TestOutput(string markdown, string expectedResult)
        {
            Markdig.Markdown.Convert(markdown, Renderer!, Pipeline);
            Assert.AreEqual(expectedResult, Document?.Result);
        }
    }
}