using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
using Microsoft.Extensions.Configuration;

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
            var styles = ReadStyles();
            var converter = new Md2WordConverter(options.MarkdownFile, options.TemplateFile, styles)
            {
                OutputDirectory = options.OutputDirectory,
                OutputFileName = options.OutputFile
            };

            converter.Convert();
        }

        private static IReadOnlyDictionary<FontStyles, string> ReadStyles()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", false, false);
           
            var config = builder.Build();
            return config.GetSection("Styles")
                .GetChildren()
                .ToDictionary(c => Enum.Parse<FontStyles>(c.Key), c => c.Value);
        }
    }
}