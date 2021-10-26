using System;
using System.IO;
using System.Net;
using System.Text;
using MD2Word.Markdown.Syntax;
using PlantUml.Net;

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
            var factory = new RendererFactory();

            var plantUmlRenderer = factory.CreateRenderer(new PlantUmlSettings());

            var image = plantUmlRenderer.Render(sb.ToString(), OutputFormat.Png);
            Document.InsertPngImage(image);
        }
    }
}