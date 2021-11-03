using System;
using System.IO;
using System.Text;

namespace MD2Word
{
    public class DocumentWriter : TextWriter
    {
        private readonly IDocument _document;
 
        private bool _isOpen;

        public DocumentWriter(IDocument document)
        {
            _document = document;
            _isOpen = true;
        }
        public override void Close()
        { 
            Dispose(true);
        }

        protected override void Dispose(bool disposing) 
        {
            // Do not destroy _sb, so that we can extract this after we are 
            // done writing (similar to MemoryStream's GetBuffer & ToArray methods) 
            _isOpen = false;
            base.Dispose(disposing); 
        }


        public override Encoding Encoding => Encoding.Unicode;

        public override void Write(char value) 
        {
            if (!_isOpen) 
                ThrowExceptionIfClosed();
            
            _document.WriteText(value.ToString());
        }

        public override void Write(char[] buffer, int index, int count) { 
            if (!_isOpen)
                ThrowExceptionIfClosed();
        
            if (buffer==null)
                throw new ArgumentNullException(nameof(buffer));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index)); 
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count)); 
            if (buffer.Length - index < count) 
                throw new ArgumentException("off len");
 
            _document.WriteText(new string(buffer, index, count));
        }

        public override void Write(string? value) 
        { 
            if (!_isOpen)
                ThrowExceptionIfClosed();
            
            if (!string.IsNullOrEmpty(value)) 
                _document.WriteText(value!);
        }

        private static void ThrowExceptionIfClosed()
        {
            throw new Exception("Writer is closed");
        }
    }
}