namespace MD2Word
{
    public interface ICell
    {
        int ColumnSpan { get; set; }
        int RowSpan { get; set; }
        void Align(Alignment alignment);
    }

    public enum Alignment
    {
        Center,
        Right,
        Left
    }
}