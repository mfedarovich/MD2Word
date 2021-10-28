using Markdig;
using MD2Word.Markdown.Parsers;
using MD2Word.Markdown.Renderers;
using NUnit.Framework;

namespace MD2Word
{
    public class BaseTest
    {
        private DocumentStub Document { get; set; } 
        private DocRenderer Renderer { get; set; }
        protected MarkdownPipeline Pipeline { get; private set; }
        
        [SetUp]
        public void Setup()
        {
            Document = new DocumentStub();
            Renderer = new DocRenderer(Document);
            
            var pipelineBuilder = new MarkdownPipelineBuilder();
            pipelineBuilder.BlockParsers.Add(new ExtendedBlockParser());
            Pipeline = pipelineBuilder.Build();
            
        }

        protected void TestOutput(string markdown, string expectedResult)
        {
            Markdig.Markdown.Convert(markdown, Renderer, Pipeline);
            Assert.AreEqual(expectedResult, Document.Result);
        }
    }
}