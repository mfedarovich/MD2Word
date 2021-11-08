using System;

namespace MD2Word
{
    public interface IImage : IDisposable
    {
        void InsertImageFromFile(string fileName);
        void InsertImageFromUrl(string url);
        void InsertUml(string umlScript);
    }
}