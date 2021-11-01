﻿using System.IO;

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
        void PushStyle(FontStyles style, bool inline);
        void PushStyle(FontStyles style, int nestingLevel);
        void PopStyle(bool inline);
        void InsertImageFromFile(string fileName);
        void InsertUml(string umlScript);
        void Emphasise(bool italic, bool bold);
    }
}