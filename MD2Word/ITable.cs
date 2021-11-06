using System;

namespace MD2Word
{
    public interface ITable : IDisposable
    {
        IRow AddRow(bool isHeader);
        void AddColumnDefinition(float width);
    }
}