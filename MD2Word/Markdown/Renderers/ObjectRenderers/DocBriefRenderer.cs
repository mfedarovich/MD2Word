using System.Linq;
using System.Text;
using Markdig.Helpers;
using MD2Word.Markdown.Syntax;

namespace MD2Word.Markdown.Renderers.ObjectRenderers
{
    public class DocBriefRenderer: DocObjectRenderer<BriefBlock>
    {
        public DocBriefRenderer(IDocument document) : base(document)
        {
        }

        protected override void Write(DocRenderer renderer, BriefBlock obj)
        {
            var text = obj.Lines.ToSlice().ToString();
            Document.WriteText(text);
        }
    }
}