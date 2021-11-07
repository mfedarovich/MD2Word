using System.IO;
using System.Text;
using FakeItEasy;

namespace MD2Word
{
    public class DocumentStub : IDocument
    {
        private readonly StringBuilder _executionLog = new StringBuilder();
        private IDocumentWriter _writer;

        public string Result => _executionLog.ToString();

        public ITable CreateTable()
        {
            _executionLog.Append("{table}");
            return A.Fake<ITable>();
        }

        public IParagraph CreateParagraph()
        {
            _writer = new ParagraphStub(_executionLog); 
            return (IParagraph)_writer;
        }

        public IInline CreateInline()
        {
            _writer = new InlineStub(_executionLog);
            return (IInline)_writer;
        }
        
        public TextWriter GetWriter()
        {
            return new DocumentWriter(this);
        }

        public void WriteText(string text)
        {
            _writer.WriteText(text);
        }

        public void WriteHyperlink(string label, string url)
        {
            _executionLog.Append($"h:{label}-{url}");
        }

        public void WriteSymbol(string htmlSymbol)
        {
            _writer.WriteSymbol(htmlSymbol);
        }

        public void WriteLine()
        {
            _writer.WriteLine();
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

        public void WriteHyperlink(string url)
        {
            _writer.WriteHyperlink(url, url);
        }
        public void Dispose()
        {
        }
    }
}