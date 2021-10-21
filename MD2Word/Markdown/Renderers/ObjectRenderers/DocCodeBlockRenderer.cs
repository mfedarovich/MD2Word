using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;

namespace MD2Word.Markdown.Renderers.ObjectRenderers
{
 
    public class DocCodeBlockRenderer : DocObjectRenderer<CodeBlock>
    {
        public DocCodeBlockRenderer(IDocument document) : base(document)
        {
        }

        protected override void Write(DocRenderer renderer, CodeBlock obj)
        {
            Document.AddNewBlock("Code");
            var text = obj.Lines.ToSlice().ToString();
            Document.WriteText(text);
            // foreach (var line in obj.CodeBlockLines)
            // {
            //     Document.WriteText(line.TriviaBefore.ToString());
            // }
        }
        // /// <summary>
        // /// Initializes a new instance of the <see cref="DocCodeBlockRenderer"/> class.
        // /// </summary>
        // public DocCodeBlockRenderer(IDocument document) : base(document)
        // {
        //     BlocksAsDiv = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        // }
        //
        // public bool OutputAttributesOnPre { get; set; }
        //
        // /// <summary>
        // /// Gets a map of fenced code block infos that should be rendered as div blocks instead of pre/code blocks.
        // /// </summary>
        // public HashSet<string> BlocksAsDiv { get; }
        //
        // protected override void Write(DocRenderer renderer, CodeBlock obj)
        // {
        //     return;
        //     
        //     renderer.EnsureLine();
        //
        //     var writer = new StringWriter(new StringBuilder());
        //     var fencedCodeBlock = obj as FencedCodeBlock;
        //     if (fencedCodeBlock?.Info != null && BlocksAsDiv.Contains(fencedCodeBlock.Info))
        //     {
        //         var infoPrefix = (obj.Parser as FencedCodeBlockParser)?.InfoPrefix ??
        //                          FencedCodeBlockParser.DefaultInfoPrefix;
        //
        //         // We are replacing the HTML attribute `language-mylang` by `mylang` only for a div block
        //         // NOTE that we are allocating a closure here
        //
        //         writer.Write("<div");
        //                 // .WriteAttributes(obj.TryGetAttributes(),
        //                 //     cls => cls.StartsWith(infoPrefix, StringComparison.Ordinal) ? cls.Substring(infoPrefix.Length) : cls)
        //         writer.Write('>');
        //         WriteLeafRawLines(writer, obj, true, true, true);
        //         writer.WriteLine("</div>");
        //
        //     }
        //     else
        //     {
        //         writer.Write("<pre");
        //
        //         // if (OutputAttributesOnPre)
        //         // {
        //         //     renderer.WriteAttributes(obj);
        //         // }
        //
        //         writer.Write("><code");
        //
        //         // if (!OutputAttributesOnPre)
        //         // {
        //         //     renderer.WriteAttributes(obj);
        //         // }
        //
        //         writer.Write('>');
        //
        //         WriteLeafRawLines(writer, obj, true, true);
        //
        //         // if (renderer.EnableHtmlForBlock)
        //         {
        //             writer.WriteLine("</code></pre>");
        //         }
        //     }
        //
        //     Document.WriteHtml(writer.ToString());
        //     renderer.EnsureLine();
        //     writer.Close();
        // }
        //
        // public static StringWriter WriteLeafRawLines(StringWriter writer, LeafBlock leafBlock, bool writeEndOfLines, bool escape, bool softEscape = false)
        // {
        //     if (leafBlock is null) throw new ArgumentNullException(nameof(leafBlock));
        //     
        //     if (leafBlock?.Lines.Lines != null)
        //     {
        //         var lines = leafBlock.Lines;
        //         var slices = lines.Lines;
        //         for (int i = 0; i < lines.Count; i++)
        //         {
        //             if (!writeEndOfLines && i > 0)
        //             {
        //                 writer.WriteLine();
        //             }
        //             if (escape)
        //             {
        //                 WriteEscape(writer, ref slices[i].Slice, softEscape);
        //             }
        //             else
        //             {
        //                 Write(writer, ref slices[i].Slice);
        //             }
        //             if (writeEndOfLines)
        //             {
        //                 writer. WriteLine();
        //             }
        //         }
        //     }
        //     return writer;
        // }
        //
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static StringWriter Write(StringWriter writer, ref StringSlice slice)
        // {
        //     if (slice.Start > slice.End)
        //     {
        //         return writer;
        //     }
        //     writer.Write(slice.Text, slice.Start, slice.Length);
        //     return writer;
        // }
        //
        // // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // // public StringWriter WriteEscape(StringWriter writer, string? content)
        // // {
        // //     if (content is { Length: > 0 })
        // //     {
        // //         WriteEscape(writer, content, 0, content.Length);
        // //     }
        // //     return writer;
        // // }
        // //
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static StringWriter WriteEscape(StringWriter writer, ref StringSlice slice, bool softEscape = false)
        // {
        //     if (slice.Start > slice.End)
        //     {
        //         return writer;
        //     }
        //     return WriteEscape(writer, slice.Text, slice.Start, slice.Length, softEscape);
        // }
        // //
        // // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // // public StringWriter WriteEscape(StringWriter writer, StringSlice slice, bool softEscape = false)
        // // {
        // //     return WriteEscape(writer, ref slice, softEscape);
        // // }
        //
        // public static StringWriter WriteEscape(StringWriter writer, string content, int offset, int length, bool softEscape = false)
        // {
        //     if (string.IsNullOrEmpty(content) || length == 0)
        //         return writer;
        //
        //     var end = offset + length;
        //     int previousOffset = offset;
        //     for (;offset < end;  offset++)
        //     {
        //         switch (content[offset])
        //         {
        //             case '<':
        //                 writer.Write(content, previousOffset, offset - previousOffset);
        //                 writer.Write("&lt;");
        //                 previousOffset = offset + 1;
        //                 break;
        //             case '>':
        //                 if (!softEscape)
        //                 {
        //                     writer.Write( content, previousOffset, offset - previousOffset);
        //                     writer.Write("&gt;");
        //                     previousOffset = offset + 1;
        //                 }
        //                 break;
        //             case '&':
        //                 writer.Write(content, previousOffset, offset - previousOffset);
        //                 writer.Write("&amp;");
        //                 previousOffset = offset + 1;
        //                 break;
        //             case '"':
        //                 if (!softEscape)
        //                 {
        //                     writer.Write(content, previousOffset, offset - previousOffset);
        //                     writer.Write("&quot;");
        //                     previousOffset = offset + 1;
        //                 }
        //                 break;
        //         }
        //     }
        //
        //     writer.Write(content, previousOffset, end - previousOffset);
        //     return writer;
        // }

    }
}