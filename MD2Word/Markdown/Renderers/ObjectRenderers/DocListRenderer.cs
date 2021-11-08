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
                for (var i = 0; i < listBlock.Count; i++)
                {
                    var item = listBlock[i];
                    var listItem = (ListItemBlock) item;
                    using var paragraph = Document.CreateParagraph();
                    paragraph.SetStyle(FontStyles.NumberList);
                    renderer.WriteChildren(listItem);
                }
            }
            else
            {
                for (var i = 0; i < listBlock.Count; i++)
                {
                    var item = listBlock[i];
                    var listItem = (ListItemBlock) item;
                    if (listItem.Count != 0)
                    {
                        using var paragraph = Document.CreateParagraph();
                        paragraph.SetStyle(FontStyles.BulletList);
                        renderer.WriteChildren(listItem);
                    }
                }
            }
        }
    }
}