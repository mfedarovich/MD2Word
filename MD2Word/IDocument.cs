using System;
using System.IO;

namespace MD2Word
{
    public interface IDocument : IDisposable
    {
        ITable CreateTable();

        IParagraph CreateParagraph();
        IInline CreateInline();
        TextWriter GetWriter();
        void InsertImageFromFile(string fileName);
        void InsertImageFromUrl(string url);
        void InsertUml(string umlScript);
        
        IDocumentWriter Writer { get; }
    }
}