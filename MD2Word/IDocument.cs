using System.IO;

namespace MD2Word
{
    public interface IDocument
    {
        void StartNextParagraph();
        TextWriter GetWriter();
        void WriteText(string text);
        void WriteInlineText(string text);
        void WriteHyperlink(string url);
        void WriteSymbol(string htmlSymbol);
        void WriteLine();
        void WriteHtml(string html);
        void PushStyle(string style, bool inline = false);
        void PopStyle();
        void InsertImageFromFile(string fileName);
        void InsertUml(string umlScript);
        void Emphasise(bool italic, bool bold);
    }
}