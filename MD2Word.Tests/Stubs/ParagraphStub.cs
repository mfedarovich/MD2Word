using System.Text;

namespace MD2Word.Stubs
{
    class ParagraphStub : BlockTextStub, IParagraph
    {
        public ParagraphStub(StringBuilder log) : base(log)
        {
            log.AppendLine("p");
        }

        public override void WriteText(string text)
        {
            Log.AppendLine($"t:{text}");
        }

        public override void SetStyle(FontStyles style, int level = 0)
        {
            base.SetStyle(style, level);
            if (level == 0)
                Log.AppendFormat("{{{0}}}", style.ToString().ToUpper());
            else
                Log.AppendFormat("{{{0}#{1}}}", style.ToString(), level);
        }

        public void CreateHorizontalRule()
        {
            Log.AppendLine("{hz rule}");
        }
    }
}