using System;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MD2Word.Word.Blocks;
using MD2Word.Word.Extensions;
using MD2Word.Word.Tables;

namespace MD2Word.Word
{
    public class Document : IDocument
    {
        private class EmptyWriter : IDocumentWriter
        {
            public void WriteText(string text){}
            public void WriteSymbol(string symbol){}
            public void WriteLine(){}
            public void WriteHyperlink(string url){}
        }
        private readonly WordprocessingDocument _doc;
        private readonly Settings _settings;
        private OpenXmlElement? _current;
        private readonly DocList _docList;
 
        private OpenXmlElement Current
        {
            get =>_current ?? _doc.GetPlaceholder("body");
            set => _current = value;
        }
        public IDocumentWriter Writer { get; private set; } 
      
        public Document(WordprocessingDocument doc, Settings settings)
        {
            _doc = doc;
            _settings = settings;
            Writer = new EmptyWriter();
            _docList = new DocList(doc);
        }
        
        public IImage CreateImage()
        {
            if (_current == null) CreateOrReuseParagraphIfEmpty();
            return new DocImage(_doc, Current, _settings.DrawIoPath);
        }

        public IParagraph CreateTitle()
        {
            return CreateSpecialParagraph("title");
        }

        public IParagraph CreateBrief()
        {
            return CreateSpecialParagraph("brief");
        }

        public ITable CreateTable()
        {
            var current = Current;
            var table = new DocTable(current, (newParent) => _current = newParent);
            return table;
        }

        public IParagraph CreateParagraph()
        {
            var paragraph = CreateOrReuseParagraphIfEmpty();
            DocParagraph docParagraph = new(_doc, paragraph, _settings.Styles, _docList);
            Writer = docParagraph;
            return docParagraph;
        }

        public IInline CreateInline()
        {
            if (Current.IsPlaceholder()) throw new FormatException("No paragraph is created before");
            
            var docInline = new DocInline(_doc, Current, _settings.Styles);
            Writer = docInline;
            return docInline;
        }
        private Paragraph CreateOrReuseParagraphIfEmpty()
        {
            var current = Current;
            if (current is Paragraph p && !current.Descendants<Run>().Any())
            {
                return p;
            }
            var paragraph = current.InsertAfterSelf(new Paragraph());
            Current = paragraph;
            return paragraph;
        }
        private IParagraph CreateSpecialParagraph(string tag)
        {
            var titlePlaceholder = _doc.GetPlaceholder(tag);
            var paragraph = titlePlaceholder.InsertAfterSelf(new Paragraph());
            var oldCurrent = _current;
            _current = paragraph;
            var docParagraph = new DocParagraph(_doc, paragraph, _settings.Styles, _docList);
            docParagraph.Closing += () => _current = oldCurrent;
            Writer = docParagraph;
            return docParagraph;
        }
        public void Dispose()
        {
            _doc.RemovePlaceholders();
            _doc.Dispose();
        }
    }
}