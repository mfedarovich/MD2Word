using System;

namespace MD2Word
{
    public interface IRow : IDisposable
    {
        ICell AddCell();
    }
}