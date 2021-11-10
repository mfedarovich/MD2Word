using System.Text;

namespace MD2Word.Stubs
{
    public abstract class BlockTextStub : BaseStub, IBlockText
    {
        private bool _styleSpecified = false;

        protected BlockTextStub(StringBuilder log) : base(log)
        {
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

        public void WriteHyperlink(string url)
        {
            Log.Append($"h:{url}");
        }

        public virtual void SetStyle(FontStyles style, int level = 0)
        {
            _styleSpecified = true;
        }

        public void Emphasise(bool italic, bool bold)
        {
        }

        public void SetForeground(string? rgb)
        {
            
        }

        public void SetBackground(string? rgb)
        {
            
        }

        public override void Dispose()
        {
            if(_styleSpecified)
                Log.Append("{!}");
        }
    }
}