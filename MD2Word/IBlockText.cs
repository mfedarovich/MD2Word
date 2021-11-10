using System;

namespace MD2Word
{
    public interface IBlockText : IDocumentWriter, IDisposable
    {
        void SetStyle(FontStyles style, int level = 0);
        void Emphasise(bool italic, bool bold);
        void SetForeground(string? rgb);
        void SetBackground(string? rgb);
    }
}