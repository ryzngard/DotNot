using DotNot.Generators.StackGenerator;
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
            var pushOperation = new PushOperation(number);
        }

        public void AddTopStackDuplication()
        {
            throw new NotImplementedException();
        }

        public void AddTopStackSwap()
        {
            throw new NotImplementedException();
        }

        public void DiscardTopStack()
        {
            throw new NotImplementedException();
        }

        public void AddAddition()
        {
            throw new NotImplementedException();
        }

        public void AddSubtraction()
        {
            throw new NotImplementedException();
        }

        public void AddMultiplication()
        {
            throw new NotImplementedException();
        }

        public void AddDivision()
        {
            throw new NotImplementedException();
        }

        public void AddModulo()
        {
            throw new NotImplementedException();
        }

        public void AddHeapStore()
        {
            throw new NotImplementedException();
        }

        public void AddHeapRetrieve()
        {
            throw new NotImplementedException();
        }

        public void OutputTopStackChar()
        {
            throw new NotImplementedException();
        }

        public void OutputTopStackNumber()
        {
            throw new NotImplementedException();
        }
    }
}
