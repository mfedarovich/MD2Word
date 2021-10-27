using System;
using Markdig.Syntax.Inlines;

namespace MD2Word.Markdown.Renderers.InlineRenderers
{
    public class DocAutolinkInlineRenderer : DocInlineRenderer<AutolinkInline>
    {
        public DocAutolinkInlineRenderer(IDocument document) : base(document)
        {
            
        }
        protected override void Write(DocRenderer renderer, AutolinkInline obj)
        {
            Document.PushStyle("InfoBlue", true);
            Document.WriteInlineText("<!-AutolinkInline-->" + Environment.NewLine);
            Document.PopStyle();

            Document.WriteText(obj.Url);
        }
    }
}