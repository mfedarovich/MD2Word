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
            using var paragraph = Document.CreateParagraph();
            if (listBlock.IsOrdered)
            {
                paragraph.SetStyle(FontStyles.NumberList);
                for (var i = 0; i < listBlock.Count; i++)
                {
                    var item = listBlock[i];
                    var listItem = (ListItemBlock) item;
                    renderer.WriteChildren(listItem);
                }
            }
            else
            {
                paragraph.SetStyle(FontStyles.BulletList);

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
        }
    }
}