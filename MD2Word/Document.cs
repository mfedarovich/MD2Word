using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MD2Word.Word.Blocks;
using MD2Word.Word.Extensions;
using MD2Word.Word.Tables;
using PlantUml.Net;

namespace MD2Word
{
    public class Document : IDocument
    {
        private readonly WordprocessingDocument _doc;
        private readonly Dictionary<FontStyles, string> _styles;
        private OpenXmlElement? _current;
        private readonly EmbeddedImage _image;
        private readonly Stack<DocStyle> _styleHistory = new();

        public IDocumentWriter? Writer { get; private set; } 

        public Document(WordprocessingDocument doc, Dictionary<FontStyles, string> styles)
        {
            _doc = doc;
            _styles = styles;
            _image = new EmbeddedImage(doc, 500);
        }

        public ITable CreateTable()
        {
            return new DocTable(_doc.MainDocumentPart?.Document.Body!, (newParent) => _current = newParent);
        }

        public IParagraph CreateParagraph()
        {
            var paragraph = CreateOrReuseParagraphIfEmpty();
            DocParagraph docParagraph;
         
            void OnDestroy() => _styleHistory.Pop();
            if (_styleHistory.Count == 0)
            {
                docParagraph = new DocParagraph(_doc, paragraph, _styles, OnDestroy);
            }
            else
            {
                docParagraph = new DocParagraph(_doc, paragraph, (DocStyle)(_styleHistory.Peek()).Clone(),
                    OnDestroy);
            }
            
            Writer = docParagraph;
            _styleHistory.Push(docParagraph.Style);
            return docParagraph;
        }

        public IInline CreateInline()
        {
            if (Current.IsPlaceholder()) throw new FormatException("No paragraph is created before");
            
            var docInline = new DocInline(_doc, Current, _styles);
            Writer = docInline;
            return docInline;
        }


        public TextWriter GetWriter()
        {
            return new DocumentWriter(this);
        }
        
        public void InsertPngImage(byte[] buffer)
        {
            CreateOrReuseParagraphIfEmpty();
            _image.AddImage(Current, buffer);
        }

        public void InsertImageFromFile(string fileName)
        {
            if (Path.GetExtension(fileName) != ".png")
                throw new FileFormatException("Only png files are supported");
            
            InsertPngImage(File.ReadAllBytes(fileName));
        }

        public void InsertImageFromUrl(string url)
        {
            using var webClient = new WebClient();
            var data = webClient.DownloadData(url);
            InsertPngImage(data);
        }

        public void InsertUml(string umlScript)
        {
            var factory = new RendererFactory();
            var plantUmlRenderer = factory.CreateRenderer(new PlantUmlSettings());
            var buffer = plantUmlRenderer.Render(umlScript, OutputFormat.Png);
            InsertPngImage(buffer);
        }
        
        private OpenXmlElement Current
        {
            get =>_current ?? _doc.GetBodyPlaceholder();
            set => _current = value;
        } 
        private Paragraph CreateOrReuseParagraphIfEmpty()
        {
            var current = Current;
            if (current is Paragraph p && !current.Descendants<Run>().Any())
            {
                return p;
            }
            var paragraph = current.InsertAfterSelf(new Paragraph());
            if (current.IsPlaceholder())
                current.Remove();
            Current = paragraph;
            return paragraph;
        }
        
        public void Dispose()
        {
            _doc?.Dispose();
        }
    }
}