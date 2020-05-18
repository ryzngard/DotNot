using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestUtilities
{
    public static class CompilationUtilities
    {
        public static CSharpCompilation CreateCompilation(string assemblyName)
        {
            return CSharpCompilation.Create(assemblyName);
        }
    }
}
