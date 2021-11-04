using CommandLine;

namespace MD2Word
{
    public class Options
    {

        [Option('t', "template", Required = true, HelpText = "Word document (*.docx), which is used as template for document generation")]
        public string TemplateFile { get; set; } = null!;

        [Option('m', "markdown", Required = true, HelpText = "Input markdown file")]
        public string MarkdownFile { get; set; } = null!;

        [Option('o', "output", Required = false, HelpText = "[Optional]: Output file name, shall be specified if output name shall differ from markdown file")]
        public string? OutputFile { get; set; }
        
        [Option('d', "dir", Required = false, HelpText = "[Optional]: Output directory, otherwise document will be generated nearby markdown file")]
        public string? OutputDirectory { get; set; }
    }
}