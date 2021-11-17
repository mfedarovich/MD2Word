using System;
using System.Collections.Generic;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using MD2Word.Markdown;
using MD2Word.Markdown.Renderers;
using MD2Word.Word;
using MD2Word.Word.Extensions;

namespace MD2Word
{
    public class Md2WordConverter
    {
        private readonly IReadOnlyDictionary<FontStyles, string> _styles;
        public string WordTemplateFile { get; }
        public string MarkdownFile { get; }
        public string CurrentDirectory => new FileInfo(MarkdownFile).DirectoryName!;

        public string? OutputFileName { get; set; }
        public string? OutputDirectory { get; set; }

        public Md2WordConverter(string markdownFile, string wordTemplateFile, IReadOnlyDictionary<FontStyles, string> styles)
        {
            _styles = styles;
            MarkdownFile = markdownFile;
            WordTemplateFile = wordTemplateFile;
        }

        public void Convert()
        {
            Directory.SetCurrentDirectory(CurrentDirectory);
            using var doc = OpenDocument();

            var pipelineBuilder = new DocMarkdownPipelineBuilder();
            var pipeline = pipelineBuilder.Build();
            Markdig.Markdown.Convert(
                GetMdContent(), 
                new DocRenderer(doc), 
                pipeline);
        }

        private string GetMdContent() => File.ReadAllText(MarkdownFile);

        private IDocument OpenDocument()
        {
            var documentFile = PrepareDocument();
            var wordDocument = WordprocessingDocument.Open(documentFile, true);
            wordDocument.SetUpdateFieldsOnOpen();
            return new Document(wordDocument, _styles);
        }

        private string PrepareDocument()
        {
            var createDocument =
                new CreateDocumentCommand(WordTemplateFile, MarkdownFile, OutputDirectory, OutputFileName);
            createDocument.Execute();
            return createDocument.OutputFile;
        }
    }
}