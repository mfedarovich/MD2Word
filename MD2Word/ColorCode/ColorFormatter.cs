using System.Collections.Generic;
using System.Linq;
using ColorCode;
using ColorCode.Common;
using ColorCode.Parsing;
using ColorCode.Styling;

namespace MD2Word.ColorCode
{
    public class ColorFormatter : CodeColorizerBase
    {
        private readonly IDocument _document;
        /// <summary>
        /// skip alpha channel and '#' symbol
        /// </summary>
        private static string? GetRgb(string? color) => color?.Substring(3);
        
        public ColorFormatter(IDocument document) : base(null, null)
        {
            _document = document;
        }

        public void Write(IEnumerable<string> lines, ILanguage language)
        {
            using var paragraph = _document.CreateParagraph();
            paragraph.SetStyle(FontStyles.CodeBlock);
            if (Styles.Contains(ScopeName.PlainText))
            {
                var plainTextStyle = Styles[ScopeName.PlainText];
                paragraph.SetForeground(GetRgb(plainTextStyle.Foreground));
                paragraph.SetBackground(GetRgb(plainTextStyle.Background));
            }

            var list = lines.ToList();
            int toIndex = list.Count - 1; 
            for (int i = toIndex; i >= 0; i--)
            {
                if (!string.IsNullOrEmpty(list[i]))
                {
                    break;
                }
                toIndex = i - 1;
            }

            for (int i = 0; i <= toIndex; i++)
            {
                var line = list[i];
                languageParser.Parse(line, language, Write);
                using var inline = _document.CreateInline();
                if (i < toIndex)
                    inline.WriteLine();
            }
        }

        protected override void Write(string parsedSourceCode, IList<Scope> scopes)
        {
            if (scopes.Count == 0)
            {
                WriteText(parsedSourceCode);
                return;
            }
            
            var styleInsertions = SplitByStyle(scopes);
            for (int i = 0; i < styleInsertions.Count - 1; ++i)
            {
                var left = styleInsertions[i];
                var right = styleInsertions[i + 1];
                WriteText(parsedSourceCode.Substring(left.Index, right.Index - left.Index), left.Scope);
            }
            var last = styleInsertions[styleInsertions.Count - 1];
            WriteText(parsedSourceCode.Substring(last.Index), last.Scope);
        }

        private static IList<TextInsertion> SplitByStyle(IEnumerable<Scope> scopes)
        {
            var styleInsertions = new List<TextInsertion>();
            foreach (var scope in scopes)
                GetStyleInsertionsForCapturedStyle(scope, styleInsertions);

            styleInsertions.SortStable((x, y) => x.Index.CompareTo(y.Index));
            return styleInsertions;
        }

        private void WriteText(string parsedSourceCode, Scope scope)
        {
            using var inline = _document.CreateInline();
            BuildSpanForCapturedStyle(inline, scope);
            inline.WriteText(parsedSourceCode);
        }

        private void WriteText(string parsedSourceCode)
        {
            using var inline = _document.CreateInline();
            inline.WriteText(parsedSourceCode);
        }


        private static void GetStyleInsertionsForCapturedStyle(Scope scope, ICollection<TextInsertion> styleInsertions)
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
            if (!Styles.Contains(scope.Name)) return;
            
            Style style = Styles[scope.Name];
            inline.Emphasise(style.Italic, style.Bold);
            inline.SetForeground(GetRgb(style.Foreground));
            inline.SetBackground(GetRgb(style.Background));
        }
    }
}