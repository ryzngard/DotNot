using System;
using System.Collections.Generic;
using System.Text;

namespace DotNot.Generators.StackGenerator
{
    internal class Label : IEquatable<Label>, ICommand
    {
        public string Name { get; }

        public Label(string name)
        {
            Name = name;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Label);
        }

        public bool Equals(Label other)
        {
            return other != null &&
                   Name == other.Name;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }

        public static bool operator ==(Label left, Label right)
        {
            return EqualityComparer<Label>.Default.Equals(left, right);
        }

        public static bool operator !=(Label left, Label right)
        {
            return !(left == right);
        }
    }
}
