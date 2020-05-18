using DotNot.Generators.StackGenerator;
using Generators.StackBasedGenerator;
using System;
using System.Collections.Generic;

namespace DotNot.Generators
{
    public class StackBasedGenerator
    {
        private readonly List<ICommand> _commands = new List<ICommand>();
        private readonly HashSet<Label> _labels = new HashSet<Label>();

        public StackBasedGenerator()
        {
        }

        public void AddLabel(string name)
        {
            var label = new Label(name);
            if (_labels.Contains(label))
                throw new ArgumentException($"{name} is already a label");

            _labels.Add(label);
            _commands.Add(label);
        }

        public void AddNumberPush(int number)
        {
            _commands.Add(new PushOperation(number));
        }

        public void AddTopStackDuplication()
        {
            _commands.Add(SimpleCommands.StackDuplicate);
        }

        public void AddTopStackSwap()
        {
            _commands.Add(SimpleCommands.StackSwap);
        }

        public void DiscardTopStack()
        {
            _commands.Add(SimpleCommands.PopStack);
        }

        public void AddAddition()
        {
            _commands.Add(SimpleCommands.Addition);
        }

        public void AddSubtraction()
        {
            _commands.Add(SimpleCommands.Subtraction);
        }

        public void AddMultiplication()
        {
            _commands.Add(SimpleCommands.Multiplication);
        }

        public void AddDivision()
        {
            _commands.Add(SimpleCommands.Division);
        }

        public void AddModulo()
        {
            _commands.Add(SimpleCommands.Modulo);
        }

        public void AddHeapStore()
        {
            _commands.Add(SimpleCommands.AddToHeap);
        }

        public void AddHeapRetrieve()
        {
            _commands.Add(SimpleCommands.RetrieveFromHeap);
        }

        public void OutputTopStackChar()
        {
            _commands.Add(SimpleCommands.OutputTopStackChar);
        }

        public void OutputTopStackNumber()
        {
            _commands.Add(SimpleCommands.OutputTopStackNumber);
        }
    }
}
