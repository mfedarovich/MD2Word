using ColorCode;
using FakeItEasy;
using MD2Word.ColorCode;
using NUnit.Framework;

namespace MD2Word
{
    [TestFixture]
    public class ColorFormatterTests
    {
        [Test]
        public void Test()
        {
            var f = new ColorFormatter(A.Fake<IDocument>());

            f.Write("public void Method()\n{\n}", Languages.CSharp);
        }
    }
}