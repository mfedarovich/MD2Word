﻿using System.IO;
using System.Text;

namespace MD2Word
{
    public class DocumentStub : IDocument
    {
        StringBuilder _executionLog = new StringBuilder();

        public string Result => _executionLog.ToString();

        public void StartNextParagraph()
        {
            _executionLog.AppendLine("p");
        }

        public TextWriter GetWriter()
        {
            return new DocumentWriter(this);
        }

        public void WriteText(string text)
        {
            _executionLog.AppendLine($"t:{text}");
        }

        public void WriteInlineText(string text)
        {
            _executionLog.Append($"[{text}]");
        }

        public void WriteLine()
        {
            _executionLog.AppendLine();
        }

        public void WriteHtml(string html)
        {
            _executionLog.AppendLine($"html:{html}");
        }

        public void PushStyle(string style, bool inline = false)
        {
            _executionLog.Append("{");
            if (inline)
                _executionLog.Append("i");
            _executionLog.AppendFormat("{0}}}", style.ToUpper());
        }

        public void PopStyle()
        {
            _executionLog.Append("{!}");
        }
        
        public void InsertImageFromFile(string fileName)
        {
            _executionLog.AppendLine($"image from file: {fileName}");
        }

        public void InsertUml(string umlScript)
        {
            _executionLog.AppendLine("image from UML");
        }

        public void WriteHyperlink(string url)
        {
            _executionLog.Append($"h:{url}");
        }

        public void Emphasise(bool italic, bool bold)
        {
        }
    }
}