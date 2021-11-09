using System.IO;
using System.Text;
using Markdig.Syntax.Inlines;

namespace MD2Word.Markdown.Renderers.InlineRenderers
{
    public class DocLinkInlineRenderer: DocInlineRenderer<LinkInline>
    {
        public DocLinkInlineRenderer(IDocument document) : base(document)
        {
        }
        
        protected override void Write(DocRenderer renderer, LinkInline link)
        {
            using var inline = Document.CreateInline();
            if (link.IsImage)
            {
                if (!string.IsNullOrEmpty(link.Title))
                {
                    WriteCaption(inline, link.Title!);
                }
                DrawImage(link);
                return;
            }

            renderer.WriteChildren(link);

            string caption = link.Title!;
            if (string.IsNullOrEmpty(caption))
            {
                caption = SerializeChildrenToString(link);
            }
            else
            {
                renderer.WriteChildren(link);
            }
            InsertHyperlink(inline, caption, link);
        }
        
        private static string SerializeChildrenToString(ContainerInline containerInline)
        {
            var sb = new StringBuilder();
            var inline = containerInline.FirstChild;
            while (inline != null)
            {
                sb.Append((inline as LiteralInline)?.Content.ToString());
                inline = inline.NextSibling;
            }

            return sb.ToString();
        }

        private void InsertHyperlink(IInline inline, string label, LinkInline link)
        {
            if (string.IsNullOrEmpty(label))
                label = link.Url!;
            inline.WriteHyperlink(label, link.Url!);
        }

        private void DrawImage(LinkInline link)
        {
            using var image = Document.CreateImage();
            if (File.Exists(link.Url))
                image.InsertImageFromFile(link.Url!);
            else
                image.InsertImageFromUrl(link.Url!);
        }

        private void WriteCaption(IInline inline, string label)
        {
            if (string.IsNullOrEmpty(label)) return;

            inline.SetStyle(FontStyles.Caption);
            inline.WriteText(label);
        }
    }
}