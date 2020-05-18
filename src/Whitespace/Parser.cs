using DotNot.Generators;
using Microsoft.CodeAnalysis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DotNot.Whitespace
{
    // Specifiation pulled from https://en.wikipedia.org/wiki/Whitespace_%28programming_language%29
    internal static class Parser
    {
        internal static string ParseFile(AdditionalText file, CancellationToken cancellationToken)
        {

            var generator = MakeGenerator(file, cancellationToken);
            return generator.ToString();
        }

        internal static StackBasedGenerator MakeGenerator(AdditionalText file, CancellationToken cancellationToken)
        {
            var generator = new StackBasedGenerator();

            var sourceText = file.GetText(cancellationToken);
            var sb = new StringBuilder();

            var charStream = sourceText.ToString().GetEnumerator();
            bool finalized = false;

            var parserActions = new Dictionary<char[], Action>(CharArrayEqualityComparer.Default)
                {
                    // [Space] Stack manipulation
                    {
                        new [] {' '}, () => OnStackManipulation(charStream, generator)
                    },

                    // [Tab][Space] Arithmetic
                    {
                        new [] {'\t', ' '}, () => OnArithmetic(charStream, generator)
                    },

                    // [Tab][Tab] Heap Access
                    {
                        new [] {'\t', '\t'}, () => OnHeapAccess(charStream, generator)
                    },

                    // [LF] Flow Control
                    {
                        new [] {'\n'}, () =>
                        {
                            finalized = OnFlowControl(charStream, generator);
                        }
                    },

                    // [Tab][LF] I/O
                    {
                        new [] {'\t', '\n'}, () => OnIO(charStream, generator)
                    }
                };

            while (!finalized)
            {
                ParseInstructionModification(parserActions, charStream);
            }

            return generator;
        }

        private static void ParseInstructionModification(Dictionary<char[], Action> parserActions, CharEnumerator charStream)
        {
            List<char> charStack = new List<char>();
            int index = 0;
            List<char> all = new List<char>();

            while(charStream.MoveNext())
            {
                charStack.Add(charStream.Current);
                all.Add(charStream.Current);
                index++;

                if (parserActions.TryGetValue(charStack.ToArray(), out var action))
                {
                    action();
                    charStack.Clear();
                }
            }

            if (charStack.Any())
            {
                throw new Exception();
            }
        }

        private static void OnIO(CharEnumerator charStream, StackBasedGenerator generator)
        {
            // [Tab][LF]	[Space][Space]	-	        Output the character at the top of the stack
            // [Tab][LF]	[Space][Tab]	-	        Output the number at the top of the stack
            // [Tab][LF]	[Tab][Space]	-	        Read a character and place it in the location given by the top of the stack
            // [Tab][LF]	[Tab][Tab]	    -	        Read a number and place it in the location given by the top of the stack

            var first = AssertGetNext(charStream);
            var second = AssertGetNext(charStream);

            if (first == ' ')
            {
                if (second == ' ')
                {
                    generator.OutputTopStackChar();
                }
                else if (second == '\t')
                {
                    generator.OutputTopStackNumber();
                }
            }
            else if (first == '\t')
            {

            }
            throw new NotImplementedException();
        }

        private static bool OnFlowControl(CharEnumerator charStream, StackBasedGenerator generator)
        {
            // [LF]	        [Space][Space]	Label	    Mark a location in the program
            // [LF]	        [Space][Tab]	Label	    Call a subroutine
            // [LF]	        [Space][LF]	    Label	    Jump to a label
            // [LF]	        [Tab][Space]	Label	    Jump to a label if the top of the stack is zero
            // [LF]	        [Tab][Tab]	    Label	    Jump to a label if the top of the stack is negative
            // [LF]	        [Tab][LF]	    -	        End a subroutine and transfer control back to the caller
            // [LF]	        [LF][LF]	    -	        End the program
            var first = AssertGetNext(charStream);
            var second = AssertGetNext(charStream);

            if (first == ' ')
            {
                switch (second)
                {
                    case ' ':
                        // get the location name and add to the generator
                        return false;

                    case '\t':
                        // Get label and call subroutine
                        return false;

                    case '\n':
                        // Get label and jump to that label
                        return false;
                }
            }
            else if (first == '\t')
            {
                switch (second)
                {
                    case ' ':
                        // Get label and jump if the top of the stack is zero
                        return false;

                    case '\t':
                        // Get label and jump if the top of the stack is negative
                        return false;
                    case '\n':
                        // End subroutine
                        return false;
                }
            }
            else if (first == '\n')
            {
                if (second == '\n')
                {
                    // End program
                    return true;
                }
            }

            throw new NotImplementedException();
        }

        private static void OnHeapAccess(CharEnumerator charStream, StackBasedGenerator generator)
        {
            // [Tab][Tab]	[Space]	        -	        Store in heap
            // [Tab][Tab]	[Tab]	        -	        Retrieve from heap
            var first = AssertGetNext(charStream);

            if (first == ' ')
            {
                generator.AddHeapStore();
                return;
            }
            else if (first == '\t')
            {
                generator.AddHeapRetrieve();
                return;
            }

            throw new NotImplementedException();
        }

        private static void OnArithmetic(CharEnumerator charStream, StackBasedGenerator generator)
        {
            // [Tab][Space]	[Space][Space]	-	        Addition
            // [Tab][Space]	[Space][Tab]	-	        Subtraction
            // [Tab][Space]	[Space][LF]	    -	        Multiplication
            // [Tab][Space]	[Tab][Space]	-	        Integer Division
            // [Tab][Space]	[Tab][Tab]	    -	        Modulo
            var first = AssertGetNext(charStream);
            var second = AssertGetNext(charStream);

            if (first == ' ')
            {
                switch (second)
                {
                    case ' ':
                        // Addition
                        generator.AddAddition();
                        break;

                    case '\t':
                        // subtraction
                        generator.AddSubtraction();
                        break;

                    case '\n':
                        // multiplication
                        generator.AddMultiplication();
                        break;
                    default:
                        throw new Exception();
                }
            }
            else if (first == '\t')
            {
                switch (second)
                {
                    case ' ':
                        // Integer division
                        generator.AddDivision();
                        break;

                    case '\t':
                        // subtraction
                        generator.AddModulo();
                        break;

                    default:
                        throw new Exception();
                }
            }
            throw new NotImplementedException();
        }

        private static void OnStackManipulation(CharEnumerator charStream, StackBasedGenerator generator)
        {
            // [Space]	    [Space]	        Number	    Push the number onto the stack
            // [Space]	    [LF][Space]	    -	        Duplicate the top item on the stack
            // [Space]	    [LF][Tab]	    -	        Swap the top two items on the stack
            // [Space]	    [LF][LF]	    -	        Discard the top item on the stack
            var first = AssertGetNext(charStream);
            if (first == ' ')
            {
                // Pushing a number onto the stack. Parse the number 
                var bitList = new List<bool>();
                while (charStream.MoveNext())
                {
                    switch (charStream.Current)
                    {
                        case ' ':
                            bitList.Add(false);
                            break;
                        case '\t':
                            bitList.Add(true);
                            break;
                        case '\n':
                            generator.AddNumberPush(MakeSignedNumber(bitList));
                            return;
                    }
                }

                if (bitList.Any())
                {
                    throw new Exception();
                }
            }
            else if (first == '\n')
            {
                switch (AssertGetNext(charStream))
                {
                    case ' ':
                        // Duplicate the top item on the stack
                        generator.AddTopStackDuplication();
                        break;

                    case '\t':
                        generator.AddTopStackSwap();
                        break;

                    case '\n':
                        generator.DiscardTopStack();
                        break;
                }
            }
            throw new NotImplementedException();
        }

        private static int MakeSignedNumber(IEnumerable<bool> bitList)
        {
            var bitArray = new BitArray(bitList.ToArray());
            var tmp = new int[1];
            bitArray.CopyTo(tmp, 0);
            return tmp[0];
        }

        private static char AssertGetNext(IEnumerator<char> charStream)
        {
            if (!charStream.MoveNext()) throw new Exception();
            return charStream.Current;
        }

        private class CharArrayEqualityComparer : IEqualityComparer<char[]>
        {
            public static CharArrayEqualityComparer Default = new CharArrayEqualityComparer();
            public bool Equals(char[] x, char[] y)
            => x.SequenceEqual(y);

            public int GetHashCode(char[] obj)
            {
                int hashCode = 0;
                foreach (var c in obj)
                {
                    hashCode = HashCode.Combine(hashCode, c.GetHashCode());
                }

                return hashCode;
            }
        }
    }
}