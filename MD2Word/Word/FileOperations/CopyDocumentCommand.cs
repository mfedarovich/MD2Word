using System;
using System.IO;

namespace MD2Word.Word.FileOperations
{
    public class CopyDocumentCommand : ICommand
    {
        protected string TemplateFile { get; }
        protected string OutputFile { get; }

        public CopyDocumentCommand(string templateFile, string outputFile)
        {
            TemplateFile = EnsureThatPathIsRooted(templateFile);
            OutputFile = EnsureThatPathIsRooted(outputFile);
            
        }
        public virtual void Execute()
        {
            // Create a copy of the template file and open the copy
            File.Copy(TemplateFile, OutputFile, true);
        }

        private static string EnsureThatPathIsRooted(string path)
        {
            if (!Path.IsPathRooted(path))
                path = Path.Combine(Environment.CurrentDirectory, path);

            return path;
        }
    }
}