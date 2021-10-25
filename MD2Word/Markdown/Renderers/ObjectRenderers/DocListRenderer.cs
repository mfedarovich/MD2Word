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
            if (listBlock.IsOrdered)
            {
                Document.PushStyle("List Number");
                for (var i = 0; i < listBlock.Count; i++)
                {
                    var item = listBlock[i];
                    var listItem = (ListItemBlock) item;
                    renderer.WriteChildren(listItem);
                }
            }
            else
            {
                Document.PushStyle("List Bullet");

                for (var i = 0; i < listBlock.Count; i++)
                {
                    var item = listBlock[i];
                    var listItem = (ListItemBlock) item;
                    if (listItem.Count != 0)
                    {
                        renderer.WriteChildren(listItem);
                    }
                }
            }

            Document.PopStyle();
        }
    }
}