using System;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MD2Word.Commands;

namespace MD2Word.Word.Commands
{
    public class CreateDocumentFromTemplate : ICommand
    {
        private readonly string _templateFile;
        private readonly string _outputFile;

        public CreateDocumentFromTemplate(string templateFile, string outputFile)
        {
            _templateFile = EnsureThatPathIsRooted(templateFile);
            _outputFile = EnsureThatPathIsRooted(outputFile);
            
        }
        public void Execute()
        {
            // Create a copy of the template file and open the copy
            File.Copy(_templateFile, _outputFile, true);
            
            using (var document = WordprocessingDocument.Open(_outputFile, true))
            {
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
                    new Uri(_templateFile, UriKind.Absolute), "relationId1");

                // Save the document
                main.Document.Save();
            }
        }

        private static string EnsureThatPathIsRooted(string path)
        {
            if (!Path.IsPathRooted(path))
                path = Path.Combine(Environment.CurrentDirectory, path);

            return path;
        }
    }
}