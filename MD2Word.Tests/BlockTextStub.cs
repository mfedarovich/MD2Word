using System.Text;

namespace MD2Word
{
    public abstract class BlockTextStub : IBlockText
    {
        private bool _styleSpecified = false;
        protected StringBuilder Log { get; }

        protected BlockTextStub(StringBuilder log)
        {
            Log = log;
        }
        public abstract void WriteText(string text);

        public void WriteSymbol(string symbol)
        {
            WriteText(symbol);
        }

        public void WriteLine()
        {
            Log.AppendLine();
        }

        public void WriteHyperlink(string label, string url)
        {
            Log.Append($"h:{url}");
        }

        public void Dispose()
        {
            if(_styleSpecified)
                Log.Append("{!}");
        }

        public virtual void SetStyle(FontStyles style, int level = 0)
        {
            _styleSpecified = true;
        }

        public void Emphasise(bool italic, bool bold)
        {
        }
    }
}