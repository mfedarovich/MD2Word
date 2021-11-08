using System.Text;
using FakeItEasy;

namespace MD2Word.Stubs
{
    public class DocumentStub : BaseStub, IDocument
    {
        public DocumentStub() : base(new StringBuilder())
        {
        }
        
        public IImage CreateImage()
        {
            return new ImageStub(Log);
        }

        public IDocumentWriter Writer { get; private set; }

        public string Result => Log.ToString();

        public IParagraph CreateTitle()
        {
            Writer = new ParagraphStub(Log); 
            return (IParagraph)Writer;
        }

        public ITable CreateTable()
        {
            Log.Append("{table}");
            return A.Fake<ITable>();
        }

        public IParagraph CreateParagraph()
        {
            Writer = new ParagraphStub(Log); 
            return (IParagraph)Writer;
        }

        public IInline CreateInline()
        {
            Writer = new InlineStub(Log);
            return (IInline)Writer;
        }
    }
}