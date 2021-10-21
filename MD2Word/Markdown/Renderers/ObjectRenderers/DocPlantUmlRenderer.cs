using MD2Word.Markdown.Syntax;

namespace MD2Word.Markdown.Renderers.ObjectRenderers
{
    public class DocPlantUmlRenderer: DocObjectRenderer<PlantUmlBlock>
    {
        public DocPlantUmlRenderer(IDocument document) : base(document)
        {
        }

        protected override void Write(DocRenderer renderer, PlantUmlBlock obj)
        {
            renderer.WriteLine("[plant uml here]");
        }
    }
}