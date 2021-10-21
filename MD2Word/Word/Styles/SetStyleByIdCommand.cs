using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MD2Word.Commands;

namespace MD2Word.Word.Styles
{
    public class SetStyleByIdCommand : ICommand
    {
        private readonly string _styleId;
        private readonly WordprocessingDocument _document;
        private readonly Paragraph _paragraph;

        public SetStyleByIdCommand(WordprocessingDocument document, Paragraph paragraph, string styleId)
        {
            _document = document;
            _paragraph = paragraph;
            _styleId = styleId;
        }
        public void Execute()
        {
            var pPr = _paragraph.Elements<ParagraphProperties>().FirstOrDefault() ?? 
                      _paragraph.PrependChild(new ParagraphProperties());
            pPr.ParagraphStyleId = new ParagraphStyleId(){Val = _styleId};
        }
    }
}