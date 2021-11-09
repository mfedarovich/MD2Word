namespace MD2Word
{
    public interface IDocumentWriter
    {
        void WriteText(string text);
        void WriteSymbol(string symbol);
        void WriteLine();
        void WriteHyperlink(string url);
    }
}