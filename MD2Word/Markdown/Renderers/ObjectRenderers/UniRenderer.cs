using Markdig.Syntax;

namespace MD2Word.Markdown.Renderers.ObjectRenderers
{
    public class UniRenderer<TObject> : DocObjectRenderer<TObject> where TObject : LeafBlock
    {
        private readonly string _style;

        public UniRenderer(IDocument document, string style) : base(document)
        {
            _style = style;
        }

        protected override void Write(DocRenderer renderer, TObject obj)
        {
            Document.AddNewBlock(_style);
            var text = obj.Lines.ToSlice().ToString();
            Document.WriteText(text);
        }
    }
}