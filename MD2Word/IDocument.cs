using System.IO;
using Markdig.Helpers;

namespace MD2Word
{
    public interface IDocument
    {
        void StartNextParagraph();
        TextWriter GetWriter();
        void WriteText(string text);
        void WriteInlineText(string text);
        void WriteHtml(string html);
        void PushStyle(string style, bool inline = false);
        void PopStyle();
        void InsertPngImage(byte[] buffer);
        void InsertImageFromFile(string fileName);
        void InsertUml(string umlScript);
    }
}