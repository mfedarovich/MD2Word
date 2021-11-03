using System;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace MD2Word.Word
{
    public class CreateDocumentCommand
    {
        public CreateDocumentCommand(string templateFile, string markdownFile, string? outputDirectory, string? desiredName)
        {
            TemplateFile = EnsureThatPathIsRooted(templateFile);
            OutputFile = CreateOutputFileName(markdownFile, outputDirectory, desiredName);
        }
        
        public string TemplateFile { get;  }
        public string OutputFile { get; private set; }
        
        public void Execute()
        {
            File.Copy(TemplateFile, OutputFile, true);
            if (IsDotxFile())
                ConvertToDocx();
        }
        
        private void ConvertToDocx()
        {
            // TODO: need to elaborate it more, it doesn't work well for now
            using var document = WordprocessingDocument.Open(OutputFile, true);
            // Change the document type to Document
            document.ChangeDocumentType(DocumentFormat.OpenXml.WordprocessingDocumentType.Document);

            var main = document.MainDocumentPart;
            var docSettings = main.DocumentSettingsPart;
            var linkToTemplate = new AttachedTemplate() { Id = "relationId1" };

            // Append the attached template to the DocumentSettingsPart
            docSettings.Settings.Append(linkToTemplate);

            // Add an ExternalRelationShip of type AttachedTemplate.
            // Specify the path of template and the relationship ID
            docSettings.AddExternalRelationship(
                "https://schemas.openxmlformats.org/officeDocument/2006/relationships/attachedTemplate",
                new Uri(TemplateFile, UriKind.Absolute), "relationId1");

            // Save the document
            main.Document.Save();
        }

        private bool IsDotxFile() =>  Path.GetExtension(TemplateFile) == ".dotx";
        

        private static string CreateOutputFileName(string markdownFile, string? outputDirectory, string? desiredName)
        {
            string fileName = desiredName ?? Path.GetFileNameWithoutExtension(markdownFile);
            string directory = outputDirectory ?? Path.GetDirectoryName(markdownFile)!;
            directory = EnsureThatPathIsRooted(directory);
            
            return Path.Combine(directory, $"{fileName}.docx");
        }

    
        private static string EnsureThatPathIsRooted(string path)
        {
            if (!Path.IsPathRooted(path))
                path = Path.Combine(Environment.CurrentDirectory, path);

            return path;
        }
    }
}