using System;
using Markdig.Renderers.Roundtrip;
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
            Document.AddNewBlock("InfoBlue");
            Document.WriteText("<!-LinkInline-->" + Environment.NewLine);

            if (link.IsImage)
            {
                renderer.Write('!');
            }

            // link text
            renderer.Write('[');
            renderer.WriteChildren(link);
            renderer.Write(']');

            if (link.Label != null)
            {
                if (link.LocalLabel == LocalLabel.Local || link.LocalLabel == LocalLabel.Empty)
                {
                    renderer.Write('[');
                    if (link.LocalLabel == LocalLabel.Local)
                    {
                        renderer.Write(link.LabelWithTrivia);
                    }

                    renderer.Write(']');
                }
            }
            else
            {
                if (link.Url != null)
                {
                    renderer.Write('(');
                    renderer.Write(link.TriviaBeforeUrl);
                    if (link.UrlHasPointyBrackets)
                    {
                        renderer.Write('<');
                    }

                    renderer.Write(link.UnescapedUrl);
                    if (link.UrlHasPointyBrackets)
                    {
                        renderer.Write('>');
                    }

                    renderer.Write(link.TriviaAfterUrl);

                    if (!string.IsNullOrEmpty(link.Title))
                    {
                        var open = link.TitleEnclosingCharacter;
                        var close = link.TitleEnclosingCharacter;
                        if (link.TitleEnclosingCharacter == '(')
                        {
                            close = ')';
                        }

                        renderer.Write(open);
                        renderer.Write(link.UnescapedTitle);
                        renderer.Write(close);
                        renderer.Write(link.TriviaAfterTitle);
                    }

                    renderer.Write(')');
                }
            }
        }
    }
}