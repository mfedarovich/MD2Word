using System.Linq;
using ColorCode;
using Markdig.Syntax;
using MD2Word.ColorCode;

namespace MD2Word.Markdown.Renderers.ObjectRenderers
{
 
    public class DocCodeBlockRenderer : DocObjectRenderer<CodeBlock>
    {
        private readonly ColorFormatter _formatter; 
        
        public DocCodeBlockRenderer(IDocument document) : base(document)
        {
            _formatter = new ColorFormatter(document);
        }

        protected override void Write(DocRenderer renderer, CodeBlock obj)
        {
            _formatter.Write(obj.Lines.Lines.Select(x => x.ToString()), Languages.CSharp);
        }
    }
}