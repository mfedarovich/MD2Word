using CommandLine;

namespace MD2Word
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default
                .ParseArguments<Options>(args)
                .WithParsed(RunConversion);
        }
        
        private static void RunConversion(Options options)
        {
            var converter = new Md2WordConverter(options.MarkdownFile, options.TemplateFile)
            {
                OutputDirectory = options.OutputDirectory,
                OutputFileName = options.OutputFile
            };

            converter.Convert();
        }
    }
}