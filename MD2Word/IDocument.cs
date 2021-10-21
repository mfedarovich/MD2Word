using System.IO;

namespace MD2Word
{
    public interface IDocument
    {
        TextWriter GetWriter();
        void WriteText(string text);
        void WriteHtml(string html);
        void PushStyle(string style, bool inline = false);
        void PopStyle();
    }
}