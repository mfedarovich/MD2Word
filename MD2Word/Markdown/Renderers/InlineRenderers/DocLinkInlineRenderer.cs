using System.IO;
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
            // link text
            renderer.WriteChildren(link);
            
            if (link.Label != null && link.LocalLabel == LocalLabel.Local)
            {
                Document.WriteText(link.Label);
            }
            else if (link.Url != null)
            {
                if (link.IsImage)
                {
                    DrawImage(link);
                }
                else
                {
                    InsertHyperlink(link);
                }
            }
        }

        private void InsertHyperlink(LinkInline link)
        {
            var title = link.Title ?? link.Url;
            Document.WriteHyperlink(link.Url);
        }

        private void DrawImage(LinkInline link)
        {
            Document.StartNextParagraph();
            if (!string.IsNullOrEmpty(link.Title))
            {
                Document.PushStyle(FontStyles.Caption, true);
                Document.WriteText(link.Title);
                Document.PopStyle(true);
            }

            if (File.Exists(link.Url))
                Document.InsertImageFromFile(link.Url);
            else
                Document.InsertImageFromUrl(link.Url);
        }
    }
}