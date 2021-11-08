﻿using System;
using System.Collections.Generic;
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
        private readonly WordprocessingDocument _doc;
        private readonly Dictionary<FontStyles, string> _styles;
        private OpenXmlElement? _current;
        private readonly Stack<DocStyle> _styleHistory = new();

        public IImage CreateImage()
        {
            return new DocImage(_doc, CreateOrReuseParagraphIfEmpty());
        }

        public IDocumentWriter? Writer { get; private set; } 

        public Document(WordprocessingDocument doc, Dictionary<FontStyles, string> styles)
        {
            _doc = doc;
            _styles = styles;
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