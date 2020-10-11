using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DotNot.DependencyInjection
{
    [Generator]
    class DependencyInjectorSourceGenerator : ISourceGenerator
    {
        public void Execute(SourceGeneratorContext context)
        {
            if (context.Compilation.SyntaxTrees.FirstOrDefault() is CSharpSyntaxTree)
            {
                var newSourceFile = Generate(context.Compilation, context.CancellationToken);
                context.AddSource("ServiceLocator.cs", newSourceFile);
            }
        }

        public void Initialize(InitializationContext context)
        {
        }

        public static SourceText Generate(Compilation compilation, CancellationToken cancellationToken)
        {
            var baseText = @"
namespace DotNot.DependencyInjection
{
    public static class ServiceLocator
    {
        public static T GetService<T>()
            => null;
    }
}";

            // Just use the first set of parse options in the trees
            var parseOptions = ((CSharpSyntaxTree)compilation.SyntaxTrees.First()).Options;

            var tmpTree = CSharpSyntaxTree.ParseText(baseText, options: parseOptions, cancellationToken: cancellationToken);
            var tmpCompilation = compilation.AddSyntaxTrees(tmpTree);

            var tmpSemanticModel = tmpCompilation.GetSemanticModel(tmpTree);
            var tmpRoot = tmpTree.GetRoot(cancellationToken);
            var serviceLocatorType = (INamedTypeSymbol) tmpSemanticModel.GetDeclaredSymbol(
                tmpRoot
                    .DescendantNodesAndSelf()
                    .First(n => n is TypeDeclarationSyntax));

            var symbolsToBeConstructed = new List<ITypeSymbol>();

            // Find references to the type in the compilation 
            foreach (var syntaxTree in compilation.SyntaxTrees)
            {
                if (syntaxTree == tmpTree)
                {
                    continue;
                }

                var root = syntaxTree.GetRoot(cancellationToken);
                var semanticModel = tmpCompilation.GetSemanticModel(syntaxTree);
                var invocationExpressions = root.DescendantNodesAndSelf()
                    .Where(n => n is InvocationExpressionSyntax)
                    .Cast<InvocationExpressionSyntax>();

                foreach (var invocationExpression in invocationExpressions)
                {
                    var invocationOperation = (IInvocationOperation)semanticModel.GetOperation(invocationExpression);

                    if (invocationOperation.Instance != null)
                    {
                        // Only static calls allowed
                        continue;
                    }

                    if (SymbolEqualityComparer.Default.Equals(invocationOperation.TargetMethod.ContainingType, serviceLocatorType))
                    {
                        var typeArgument = invocationOperation.TargetMethod.TypeArguments.First();
                        symbolsToBeConstructed.Add(typeArgument);
                    }
                }
            }

            // Generate the source
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(
@"namespace DotNot.DependencyInjection
{
    public static class ServiceLocator
    {
        public static T GetService<T>()
        {
            var serviceType = typeof(T);

");

            foreach(var symbolToBeConstructed in symbolsToBeConstructed.Distinct(SymbolEqualityComparer.Default))
            {
                stringBuilder.AppendLine($@"
            if (serviceType == typeof({symbolToBeConstructed.Name}))
            {{
                return new {symbolToBeConstructed.Name};
            }}
");
            }

            stringBuilder.AppendLine(@"
            throw new Exception(""type not found"");
        }
    }
}");

            return SourceText.From(stringBuilder.ToString());
        }
    }
}
