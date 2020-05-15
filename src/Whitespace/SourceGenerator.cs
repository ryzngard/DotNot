using DotNot.Whitespace;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Whitespace
{
    [Generator]
    class SourceGenerator : ISourceGenerator
    {
        public void Execute(SourceGeneratorContext context)
        {
            foreach (var file in context.AdditionalFiles)
            {
                var extension = Path.GetExtension(file.Path);
                if (extension == ".ws")
                {
                    var sourceText = Parser.ParseFile(file, context.CancellationToken);
                    if (sourceText is null)
                    {
                        continue;
                    }

                    context.AddSource(file.Path, SourceText.From(sourceText, Encoding.UTF8));
                }
            }
        }

        public void Initialize(InitializationContext context)
        {
            // No initialization needed
        }
    }
}
