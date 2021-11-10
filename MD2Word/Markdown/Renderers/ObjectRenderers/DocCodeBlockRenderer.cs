using System.Text;
using ColorCode;
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
            // using var paragraph = Document.CreateParagraph();
            // using var inline = Document.CreateInline();
            // inline.SetStyle(FontStyles.CodeBlock);
            //
            // foreach (var line in obj.Lines)
            // {
            //     var lineOfCode = line.ToString();
            //
            //     if (!string.IsNullOrEmpty(lineOfCode))
            //     {
            //         inline.WriteText(lineOfCode);
            //         inline.WriteLine();
            //     }
            // }

            StringBuilder sb = new();
            foreach (var line in obj.Lines.Lines)
            {
                sb.AppendLine(line.ToString());
            }

            var formatter = new ColorCode.ColorFormatter(Document);
            formatter.Write(sb.ToString(), Languages.CSharp);
        }
    }
}