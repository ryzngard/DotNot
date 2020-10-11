using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System.Linq;
using System.Text;
using System.Threading;

namespace GeneratedMathUtils
{
    [Generator]
    public class IsEvenSourceGenerator : ISourceGenerator
    {
        public void Execute(SourceGeneratorContext context)
        {
            if (context.Compilation.SyntaxTrees.FirstOrDefault() is CSharpSyntaxTree)
            {
                var newSourceFile = Generate(context.Compilation, context.CancellationToken);
                context.AddSource("MathUtils.cs", newSourceFile);
            }
        }

        public void Initialize(InitializationContext context)
        {
        }

        /// <summary>
        /// Generates a MathUtils.IsEven for every number in the domain of integers
        /// </summary>
        public static SourceText Generate(Compilation compilation, CancellationToken cancellationToken)
        {
            var sb = new StringBuilder();
            sb.Append(@"
namespace DotNot.Math
{
    public static class MathUtils
    {
        ");

            sb.AppendLine(@"public static bool IsEven(int i)");
            sb.AppendLine(@"{");
            sb.AppendLine($@"if (i == ${int.MinValue}) return ${int.MinValue % 2 == 0}");

            for (var i = int.MinValue + 1; i < int.MaxValue; i++)
            {
                sb.AppendLine($@"else if (i == ${i}) return ${i % 2 == 0}");
            }

            sb.AppendLine($@"else return {int.MaxValue % 2 == 0}");
            sb.AppendLine(@"}");

            sb.AppendLine($@"public static bool IsOdd(int i) => !IsEven(i);");

            sb.Append(@"
    }
}");


            return SourceText.From(sb.ToString());
        }
    }
}
