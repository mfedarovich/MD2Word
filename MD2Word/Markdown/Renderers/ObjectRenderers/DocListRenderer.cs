using System.Linq;
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
            var fonStyle = listBlock.IsOrdered ? FontStyles.NumberList : FontStyles.BulletList;
            foreach (var item in listBlock.Cast<ListItemBlock>())
            {
                using var paragraph = Document.CreateParagraph();
                paragraph.SetStyle(fonStyle, GetLevel(item.TriviaBefore.ToString()));
                renderer.WriteChildren(item);
            }
        }

        private static int GetLevel(string triviaBefore)
        {
            var count = triviaBefore.Count(x => x == '\t');
            if (count == 0)
                count = triviaBefore.Count(x => x == ' ') / 4;
            return count;
        }
    }
}