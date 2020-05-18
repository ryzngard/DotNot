using DotNot.Generators.StackGenerator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Generators.StackBasedGenerator
{
    internal static class SimpleCommands
    {
        private class SimpleCommand : ICommand
        {
            public string Name { get; }
            public SimpleCommand(string name)
            {
                Name = name;
            }
        }

        internal static readonly ICommand AddToHeap = new SimpleCommand(nameof(AddToHeap));
        internal static readonly ICommand RetrieveFromHeap = new SimpleCommand(nameof(RetrieveFromHeap));
        internal static readonly ICommand OutputTopStackChar = new SimpleCommand(nameof(OutputTopStackChar));
        internal static readonly ICommand OutputTopStackNumber = new SimpleCommand(nameof(OutputTopStackNumber));
        internal static readonly ICommand Modulo = new SimpleCommand(nameof(Modulo));
        internal static readonly ICommand Division = new SimpleCommand(nameof(Division));
        internal static readonly ICommand Multiplication = new SimpleCommand(nameof(Multiplication));
        internal static readonly ICommand Subtraction = new SimpleCommand(nameof(Subtraction));
        internal static readonly ICommand Addition = new SimpleCommand(nameof(Addition));
        internal static readonly ICommand PopStack = new SimpleCommand(nameof(PopStack));
        internal static readonly ICommand StackSwap = new SimpleCommand(nameof(StackSwap));
        internal static readonly ICommand StackDuplicate = new SimpleCommand(nameof(StackDuplicate));
    }
}
