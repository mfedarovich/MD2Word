using System;

namespace MD2Word
{
    public interface ICell : IDisposable
    {
        int ColumnSpan { get; set; }
        int RowSpan { get; set; }
        void Align(CellAlignment cellAlignment);
    }
}