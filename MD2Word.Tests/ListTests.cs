using NUnit.Framework;

namespace MD2Word
{
    [TestFixture]
    public class ListTests : BaseTest
    {
        [TestCase("* 1\r\n* 2", "1", "2")]
        public void BulletListTest(string bulletList, string row1, string row2)
        {
            var expected = $"p\r\n{{BULLETLIST}}p\r\n[{row1}]{{!}}p\r\n{{BULLETLIST}}p\r\n[{row2}]{{!}}";
            TestOutput(bulletList, expected);
        }
        
        [TestCase("1. 1\r\n2. 2", "1", "2")]
        public void NumberListTest(string bulletList, string row1, string row2)
        {
            var expected = $"p\r\n{{NUMBERLIST}}p\r\n[{row1}]{{!}}p\r\n{{NUMBERLIST}}p\r\n[{row2}]{{!}}";
            TestOutput(bulletList, expected);
        }
    }
}