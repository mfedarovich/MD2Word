using System.Text;
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
            var sb = new StringBuilder();
            foreach (var line in obj.Lines)
            {
                sb.AppendLine(line!.ToString());
            }

            using var image = Document.CreateImage();
            image.InsertUml(sb.ToString());
        }
    }
}