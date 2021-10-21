using Markdig.Helpers;
using Markdig.Syntax;

namespace MD2Word.Markdown.Renderers.ObjectRenderers
{
    public class DocListRenderer : DocObjectRenderer<ListBlock>
    {
        public DocListRenderer(IDocument document) : base(document)
        {
            
        }
        protected override void Write(DocRenderer renderer, ListBlock listBlock)
        {
            renderer.RenderLinesBefore(listBlock);
            if (listBlock.IsOrdered)
            {
                Document.AddNewBlock("List Number");
                for (var i = 0; i < listBlock.Count; i++)
                {
                    var item = listBlock[i];
                    var listItem = (ListItemBlock) item;
                    renderer.RenderLinesBefore(listItem);

                    var bws = listItem.TriviaBefore.ToString();
                    var bullet = listItem.SourceBullet.ToString();
                    var delimiter = listBlock.OrderedDelimiter;
                    renderer.PushIndent(new string[] { $"{bws}{bullet}{delimiter}" });
                    renderer.WriteChildren(listItem);
                    renderer.RenderLinesAfter(listItem);
                }
            }
            else
            {
                Document.AddNewBlock("List Bullet");

                for (var i = 0; i < listBlock.Count; i++)
                {
                    var item = listBlock[i];
                    var listItem = (ListItemBlock) item;
                    renderer.RenderLinesBefore(listItem);

                    StringSlice bws = listItem.TriviaBefore;
                    char bullet = listBlock.BulletType;
                    StringSlice aws = listItem.TriviaAfter;

                    renderer.PushIndent(new string[] { $"{bws}{bullet}{aws}" });
                    if (listItem.Count == 0)
                    {
                        renderer.Write(""); // trigger writing of indent
                    }
                    else
                    {
                        renderer.WriteChildren(listItem);
                    }
                    renderer.PopIndent();

                    renderer.RenderLinesAfter(listItem);
                }
            }

            renderer.RenderLinesAfter(listBlock);
        }
    }
}