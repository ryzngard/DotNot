using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TestUtilities
{
    public class AdditionalTextImpl : AdditionalText
    {
        private readonly string _path;
        private readonly SourceText _sourceText;

        public AdditionalTextImpl(string path, string text)
        {
            _path = path;
            _sourceText = SourceText.From(text);
        }

        public override string Path => _path;

        public override SourceText? GetText(CancellationToken cancellationToken = default) => _sourceText;
    }
}
