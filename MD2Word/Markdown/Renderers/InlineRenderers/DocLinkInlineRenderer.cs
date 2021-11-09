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
            renderer.WriteChildren(link);
            string caption;
            if (link.IsImage)
            {
                if (!string.IsNullOrEmpty(link.Title))
                {
                    WriteCaption(link.Title!);
                    renderer.WriteChildren(link);
                }
                else
                {
                    caption = SerializeChildrenToString(link);
                    if (caption.Length > 0)
                        WriteCaption(caption);
                }
                DrawImage(link);
                return;
            }

            if (string.IsNullOrEmpty(link.Url))
            {
                renderer.WriteChildren(link);
                return;
            }
            
            caption = link.Title!;
            if (string.IsNullOrEmpty(caption))
            {
                caption = SerializeChildrenToString(link);
            }
            else
            {
                renderer.WriteChildren(link);
            }
            InsertHyperlink(caption, link);
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

        private void InsertHyperlink(string label, LinkInline link)
        {
            using var inline = Document.CreateInline();
            if (string.IsNullOrEmpty(label))
                label = link.Url!;
            inline.WriteHyperlink(label, link.Url!);
        }

        private void DrawImage(LinkInline link)
        {
            Document.CreateParagraph();
            
            using var image = Document.CreateImage();
            if (File.Exists(link.Url))
                image.InsertImageFromFile(link.Url!);
            else
                image.InsertImageFromUrl(link.Url!);
        }

        private void WriteCaption(string label)
        {
            if (string.IsNullOrEmpty(label)) return;

            using var inline = Document.CreateInline();
            inline.SetStyle(FontStyles.Caption);
            inline.WriteText(label);
        }
    }
}