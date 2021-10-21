using System;
using Markdig.Renderers.Roundtrip;
using Markdig.Syntax.Inlines;

namespace MD2Word.Markdown.Renderers.InlineRenderers
{
    public class DocCodeInlineRenderer : DocInlineRenderer<CodeInline>
    {
        public DocCodeInlineRenderer(IDocument document) : base(document)
        {
        }

        protected override void Write(DocRenderer renderer, CodeInline obj)
        {
            Document.AddNewBlock("InfoBlue");
            Document.WriteText("<!-CodeInline-->" + Environment.NewLine);

            var delimiterRun = new string(obj.Delimiter, obj.DelimiterCount);
            renderer.Write(delimiterRun);
            if (obj.Content is { Length: > 0 })
            {
                renderer.Write(obj.ContentWithTrivia);
            }
            renderer.Write(delimiterRun);
        }
    }
}