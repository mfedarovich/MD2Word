using System;
using Markdig.Renderers.Roundtrip;
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
            Document.PushStyle("InfoBlue");
            Document.WriteText("<!-AutolinkInline-->" + Environment.NewLine);
            Document.PopStyle();

            Document.WriteText(obj.Url);
        }
    }
}