using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MD2Word.Commands;

namespace MD2Word.Word.Styles
{
    public class UpdateFieldsOnOpenCommand : ICommand
    {
        private readonly WordprocessingDocument _document;

        public UpdateFieldsOnOpenCommand(WordprocessingDocument document)
        {
            _document = document;
        }
        public void Execute()
        {
            var settingsPart = _document.MainDocumentPart.DocumentSettingsPart;
            settingsPart.Settings.Append(new UpdateFieldsOnOpen() { Val = true });
        }
    }
}