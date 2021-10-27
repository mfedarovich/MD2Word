using System;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using Markdig;
using MD2Word.Markdown.Parsers;
using MD2Word.Markdown.Renderers;
using MD2Word.Word;
using MD2Word.Word.FileOperations;

namespace MD2Word
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            //OpenAndAddTextToWordDocument("Technical Design Overview2.docx", "Technical Design Overview.docx", "this is my test");

            var docName = "#Technical Design Overview-output.docx";
            var createDocument =
                new CopyDocumentCommand("Technical Design Overview.docx", docName);

            try
            {
                createDocument.Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            using (var doc = WordprocessingDocument.Open(docName, true))
            {
                var mdFile =
                    @"d:\Projects\Tecan\Improvements\Documentation\Markdown\MD2Word.App\MD2Word.App\bin\Debug\netcoreapp3.1\Graph.md";
                var mdContent = File.ReadAllText(mdFile);
                var pipelineBuilder = new MarkdownPipelineBuilder();
                pipelineBuilder.EnableTrackTrivia();
                pipelineBuilder.BlockParsers.Add(new ExtendedBlockParser());
                MarkdownPipeline pipeline = pipelineBuilder.Build();
                Markdig.Markdown.Convert(mdContent, new DocRenderer(new Document(doc)), pipeline);
                doc.SetUpdateFieldsOnOpen();
            }
            
            
            // md worklab

        }
        //
        // public static void Update(this WordprocessingDocument doc)
        // {
        //     var settingsPart = doc.MainDocumentPart.DocumentSettingsPart;
        //     settingsPart.Settings.Append(new UpdateFieldsOnOpen() { Val = true });
        // }
    }
}