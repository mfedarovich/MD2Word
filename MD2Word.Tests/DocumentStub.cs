using System.IO;
using System.Text;
using FakeItEasy;
using MD2Word.Word;

namespace MD2Word
{
    public class DocumentStub : IDocument
    {
        private readonly StringBuilder _executionLog = new StringBuilder();
        public IDocumentWriter Writer { get; private set; }

        public string Result => _executionLog.ToString();

        public ITable CreateTable()
        {
            _executionLog.Append("{table}");
            return A.Fake<ITable>();
        }

        public IParagraph CreateParagraph()
        {
            Writer = new ParagraphStub(_executionLog); 
            return (IParagraph)Writer;
        }

        public IInline CreateInline()
        {
            Writer = new InlineStub(_executionLog);
            return (IInline)Writer;
        }
        
        public TextWriter GetWriter()
        {
            return new DocumentWriter(this);
        }
        public void InsertImageFromFile(string fileName)
        {
            _executionLog.AppendLine($"img-file:{fileName}");
        }

        public void InsertImageFromUrl(string url)
        {
            _executionLog.AppendLine($"img-url:{url}");
        }

        public void InsertUml(string umlScript)
        {
            _executionLog.AppendLine("image from UML");
        }
        public void Dispose()
        {
        }
    }
}