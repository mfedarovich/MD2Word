using System;

namespace MD2Word
{
    public interface IDocument : IDisposable
    {
        IParagraph CreateTitle();
        IParagraph CreateBrief();
        ITable CreateTable();
        IParagraph CreateParagraph();
        IInline CreateInline();
        IImage CreateImage();
        IDocumentWriter Writer { get; }
    }
}