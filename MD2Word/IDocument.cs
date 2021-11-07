using System;
using System.Data;
using System.IO;

namespace MD2Word
{
    public interface IDocument : IDisposable
    {
        ITable CreateTable();

        IParagraph CreateParagraph();
        IInline CreateInline();
        TextWriter GetWriter();
        void WriteText(string text);
        void WriteHyperlink(string url);
        void WriteHyperlink(string label, string url);
        void WriteSymbol(string htmlSymbol);
        void WriteLine();
        void InsertImageFromFile(string fileName);
        void InsertImageFromUrl(string url);
        void InsertUml(string umlScript);
    }
}