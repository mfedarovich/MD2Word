using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using ColorCode;
using ColorCode.Common;
using ColorCode.HTML.Common;
using ColorCode.Parsing;
using ColorCode.Styling;

namespace MD2Word.ColorCode
{
    public class ColorFormatter : CodeColorizerBase
    {
        private readonly IDocument _document;

        public ColorFormatter(IDocument document) : base(null, null)
        {
            _document = document;
        }
        
        public void Write(string sourceCode, ILanguage language)
        {
            using var paragraph = _document.CreateParagraph();
            paragraph.SetStyle(FontStyles.CodeBlock);
            if (Styles.Contains(ScopeName.PlainText))
            {
                var plainTextStyle = Styles[ScopeName.PlainText];
                paragraph.SetForeground(plainTextStyle.Foreground?.Substring(3));
                paragraph.SetBackground(plainTextStyle.Background?.Substring(3));
            }

            languageParser.Parse(sourceCode, language, Write);
        }

        protected override void Write(string parsedSourceCode, IList<Scope> scopes)
        {
            switch (scopes.Count)
            {
                case 0:
                {
                    using var inline = _document.CreateInline();
                    using var reader = new StringReader(parsedSourceCode);
                    while (true)
                    {
                        var line = reader.ReadLine();
                        if (line is null)
                            break;
                        inline.WriteText(line);
                        inline.WriteLine();
                    }
                    inline.WriteText(reader.ReadLine()!);

                    return;
                }
                case 1:
                {
                    using var inline = _document.CreateInline();
                    BuildSpanForCapturedStyle(inline, scopes[0]);
                    inline.WriteText(parsedSourceCode);
                    return;
                }
            }

            var styleInsertions = new List<TextInsertion>();
            foreach (Scope scope in scopes)
                GetStyleInsertionsForCapturedStyle(scope, styleInsertions);

            styleInsertions.SortStable((x, y) => x.Index.CompareTo(y.Index));

            for (int i = 0; i < styleInsertions.Count - 1; ++i)
            {
                var left = styleInsertions[i];
                var right = styleInsertions[i + 1];
                using var inline = _document.CreateInline();
                var text = parsedSourceCode.Substring(left.Index, right.Index - left.Index);
                BuildSpanForCapturedStyle(inline, left.Scope);
                inline.WriteText(text);
            }

            using var lastInline = _document.CreateInline();
            var last = styleInsertions[styleInsertions.Count - 1];
            BuildSpanForCapturedStyle(lastInline, last.Scope);
            lastInline.WriteText(parsedSourceCode.Substring(last.Index));
        }

        private void GetStyleInsertionsForCapturedStyle(Scope scope, ICollection<TextInsertion> styleInsertions)
        {
            styleInsertions.Add(new TextInsertion
            {
                Index = scope.Index,
                Scope = scope
            });         

            Stack<Scope> scopes = new();
            scopes.Push(scope);
            while (scopes.Count > 0)
            {
                var s = scopes.Pop();
                foreach (var child in s.Children)
                {
                    styleInsertions.Add(new TextInsertion
                    {
                        Index = child.Index,
                        Scope = child
                    });
                    scopes.Push(child);
                }
            }
        }

        private void BuildSpanForCapturedStyle(IInline inline, Scope scope)
        {
            if (Styles.Contains(scope.Name))
            {
                Style style = Styles[scope.Name];

                inline.Emphasise(style.Italic, style.Bold);
                inline.SetForeground(style.Foreground?.Substring(3));
                inline.SetBackground(style.Background?.Substring(3));
            }
        }
    }
}