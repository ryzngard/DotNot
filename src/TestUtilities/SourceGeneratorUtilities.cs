using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace DotNot.TestUtilities
{
    public static class SourceGeneratorUtilities
    {
        public static CSharpGeneratorDriver CreateDriver(IEnumerable<ISourceGenerator> sourceGenerators, IEnumerable<AdditionalText> additionalFiles, CSharpParseOptions? parseOptions = null)
        {
            parseOptions ??= CSharpParseOptions.Default;

            return new CSharpGeneratorDriver(
                parseOptions,
                sourceGenerators.ToImmutableArray(),
                additionalTexts: additionalFiles.ToImmutableArray());
        }
    }
}
