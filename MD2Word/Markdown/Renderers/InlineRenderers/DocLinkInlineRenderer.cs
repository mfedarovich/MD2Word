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
            if (link.Url == null)
            {
                renderer.WriteChildren(link);
                return;
            }
            string caption;
            if (link.IsImage)
            {
                if (!string.IsNullOrEmpty(link.Title))
                {
                    WriteCaption(link.Title);
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
            
            caption = link.Title;
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
            if (containerInline is null)
            {
                return string.Empty;
            }

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
            if (string.IsNullOrEmpty(label))
                label = link.Url;
            Document.WriteHyperlink(label, link.Url);
        }

        private void DrawImage(LinkInline link)
        {
            Document.StartNextParagraph();
            if (File.Exists(link.Url))
                Document.InsertImageFromFile(link.Url);
            else
                Document.InsertImageFromUrl(link.Url);
        }

        private void WriteCaption(string label)
        {
            if (string.IsNullOrEmpty(label)) return;
            
            Document.PushStyle(FontStyles.Caption, true);
            Document.WriteInlineText(label);
            Document.PopStyle(true);
        }
    }
}