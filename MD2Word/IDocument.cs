﻿using System;
using System.Data;
using System.IO;

namespace MD2Word
{
    public interface IDocument : IDisposable
    {
        ITable CreateTable();

        void CreateParagraph();
        TextWriter GetWriter();
        void WriteText(string text);
        void WriteInlineText(string text);
        void WriteHyperlink(string url);
        void WriteHyperlink(string label, string url);
        void WriteSymbol(string htmlSymbol);
        void WriteLine();
        void PushStyle(FontStyles style, bool inline);
        void PushStyle(FontStyles style, int nestingLevel);
        void PopStyle(bool inline);
        void InsertImageFromFile(string fileName);
        void InsertImageFromUrl(string url);
        void InsertUml(string umlScript);
        void Emphasise(bool italic, bool bold);
    }
}