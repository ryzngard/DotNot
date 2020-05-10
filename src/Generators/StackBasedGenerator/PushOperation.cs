using System;
using System.Collections.Generic;
using System.Text;

namespace DotNot.Generators.StackGenerator
{
    internal class PushOperation : ICommand
    {
        public int Number { get; }
        public PushOperation(int number)
        {
            Number = number;
        }
    }
}
