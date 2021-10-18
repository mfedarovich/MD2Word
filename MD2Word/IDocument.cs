using System.IO;

namespace MD2Word
{
    public interface IDocument
    {
        TextWriter GetWriter();
        void AddNewBlock(string style);
        void WriteText(string text);
    }
}