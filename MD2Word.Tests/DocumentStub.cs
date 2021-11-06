﻿using System.IO;
using System.Text;
using FakeItEasy;

namespace MD2Word
{
    public class DocumentStub : IDocument
    {
        StringBuilder _executionLog = new StringBuilder();

        public string Result => _executionLog.ToString();

        public ITable CreateTable()
        {
            _executionLog.Append("{table}");
            return A.Fake<ITable>();
        }

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

        public void WriteHyperlink(string label, string url)
        {
            _executionLog.Append($"h:{label}-{url}");
        }

        public void WriteSymbol(string htmlSymbol)
        {
            WriteInlineText(htmlSymbol);
        }

        public void WriteLine()
        {
            _executionLog.AppendLine();
        }
        
        public void PushStyle(FontStyles style, bool inline)
        {
            _executionLog.Append("{");
            if (inline)
                _executionLog.Append("i");
            _executionLog.AppendFormat("{0}}}", style.ToString().ToUpper());
        }

        public void PushStyle(FontStyles style, int nestingLevel)
        {
            _executionLog.AppendFormat("{{{0}#{1}}}", style, nestingLevel);
        }

        public void PopStyle(bool inline)
        {
            _executionLog.Append("{!}");
        }
        
        public void InsertImageFromFile(string fileName)
        {
            _executionLog.AppendLine($"img-file:{fileName}");
        }

        public void InsertImageFromUrl(string url)
        {
            _executionLog.AppendLine($"img-url:{url}");
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

        public void Dispose()
        {
        }
    }
}