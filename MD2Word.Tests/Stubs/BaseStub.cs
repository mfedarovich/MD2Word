using System;
using System.Text;

namespace MD2Word.Stubs
{
    public abstract class BaseStub : IDisposable
    {
        protected BaseStub(StringBuilder log)
        {
            Log = log;
        }

        protected StringBuilder Log { get; }

        public virtual void Dispose()
        {
           
        }
    }
}