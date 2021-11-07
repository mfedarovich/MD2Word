using Markdig.Syntax;

namespace MD2Word.Markdown.Renderers.ObjectRenderers
{
 
    public class DocCodeBlockRenderer : DocObjectRenderer<CodeBlock>
    {
        public DocCodeBlockRenderer(IDocument document) : base(document)
        {
        }

        protected override void Write(DocRenderer renderer, CodeBlock obj)
        {
            Document.PushStyle(FontStyles.CodeBlock, true);
            Document.CreateParagraph();

            foreach (var line in obj.Lines)
            {
                var lineOfCode = line.ToString();

                if (!string.IsNullOrEmpty(lineOfCode))
                {
                    Document.WriteInlineText(lineOfCode);
                    Document.WriteLine();
                }
            }
            Document.PopStyle(true);
        }
    }
}