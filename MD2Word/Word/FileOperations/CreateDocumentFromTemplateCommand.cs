using System;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace MD2Word.Word.Commands
{
    public class CreateDocumentFromTemplateCommand : CopyDocumentCommand
    {
        public CreateDocumentFromTemplateCommand(string templateFile, string outputFile) : base(templateFile, outputFile)
        {
        }
        public override void Execute()
        {
            base.Execute();
            
            using (var document = WordprocessingDocument.Open(OutputFile, true))
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
                    new Uri(TemplateFile, UriKind.Absolute), "relationId1");

                // Save the document
                main.Document.Save();
            }
        }
    }
}