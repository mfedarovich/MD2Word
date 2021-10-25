using System;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using Markdig;
using MD2Word.Markdown.Parsers;
using MD2Word.Markdown.Renderers;
using MD2Word.Word;
using MD2Word.Word.Commands;

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
                    @"d:\Projects\Tecan\Improvements\Documentation\Markdown\MD2Word\MD2Word\bin\Debug\netcoreapp3.1\Graph.md";
                var mdContent = File.ReadAllText(mdFile);
                mdContent = @"### UIPipeline

`UIPipeline` is responsible for 

* managing the oscilloscope entire pipeline (start, stop), pipeline can be stopped when oscilloscope is detached from visual tree (as example)
* managing oscilloscope's renderer (initialize/de-initialize), renderer can be de-initialized when oscilloscope is detached from visual tree to decrease amount of resources in-use (for example)
* handling renderor/D3DImage errors 
* recovery from renderor/D3DImage errors
* handling RDP
* handling sleep mode use-cases
* handling display modes 
* handling monitors Plug-and-Play

There were multiple bugs related to sleep mode, docking stationgs, switching display modes, adding/removing additional monitors, failures on specific video cards or on specific drivers. The major part of such TFS calls handling incapsulated in `UIPipeline` error handing / recovery logic.
";
                var pipelineBuilder = new MarkdownPipelineBuilder();
                pipelineBuilder.EnableTrackTrivia();
                pipelineBuilder.BlockParsers.Add(new BriefBlockParser());
                pipelineBuilder.BlockParsers.Add(new PlantUmlParser());
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