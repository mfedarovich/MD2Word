using System;
using Markdig.Renderers.Roundtrip;
using Markdig.Syntax.Inlines;

namespace MD2Word.Markdown.Renderers.InlineRenderers
{
    public class DocEmphasisInlineRenderer: DocInlineRenderer<EmphasisInline>
    {
        public DocEmphasisInlineRenderer(IDocument document) : base(document)
        {
        }

        protected override void Write(DocRenderer renderer, EmphasisInline obj)
        {
            Document.PushStyle("InfoBlue");
            Document.WriteText("<!-EmphasisInline-->" + Environment.NewLine);
            Document.PopStyle();

            var emphasisText = new string(obj.DelimiterChar, obj.DelimiterCount);
            renderer.Write(emphasisText);
            renderer.WriteChildren(obj);
            renderer.Write(emphasisText);
        }
    }
}