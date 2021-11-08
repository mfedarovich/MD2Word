using System.Text;

namespace MD2Word.Stubs
{
    class ImageStub : BaseStub, IImage
    {
        public ImageStub(StringBuilder log) : base(log)
        {
        }
        
        public void InsertImageFromFile(string fileName)
        {
            Log.AppendLine($"img-file:{fileName}");
        }

        public void InsertImageFromUrl(string url)
        {
            Log.AppendLine($"img-url:{url}");
        }

        public void InsertUml(string umlScript)
        {
            Log.AppendLine("image from UML");
        }
    }
}