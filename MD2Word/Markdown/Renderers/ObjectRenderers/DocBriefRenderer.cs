﻿using MD2Word.Markdown.Syntax;

namespace MD2Word.Markdown.Renderers.ObjectRenderers
{
    public class DocBriefRenderer: DocObjectRenderer<BriefBlock>
    {
        public DocBriefRenderer(IDocument document) : base(document)
        {
        }

        protected override void Write(DocRenderer renderer, BriefBlock obj)
        {
            using var paragraph = Document.CreateParagraph();
            var text = obj.Lines.ToSlice().ToString();
            paragraph.WriteText(text);
        }
    }
}