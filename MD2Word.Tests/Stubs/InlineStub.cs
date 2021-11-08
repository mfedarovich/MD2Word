using System.Text;

namespace MD2Word.Stubs
{
    class InlineStub : BlockTextStub, IInline
    {
        public InlineStub(StringBuilder log) : base(log)
        {
        }

        public override void WriteText(string text)
        {
            Log.Append($"[{text}]");
        }

        public override void SetStyle(FontStyles style, int level = 0)
        {
            base.SetStyle(style, level);
            Log.AppendFormat("{{i{0}}}", style.ToString().ToUpper());
        }
    }
}