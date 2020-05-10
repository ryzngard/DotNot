using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DotNot.Parsers
{
    public class InstructionModificationParser<T>
    {
        private class SequenceComparer : IEqualityComparer<T[]>
        {
            public bool Equals(T[] x, T[] y)
                => x.SequenceEqual(y);

            public int GetHashCode(T[] obj)
                => obj.GetHashCode();
        }

        private ImmutableDictionary<T[], Action<InstructionModificationParser<T>>> _actions;
        private bool _finalized = false;

        public InstructionModificationParser(IDictionary<T[], Action<InstructionModificationParser<T>>> actions, IEqualityComparer<T[]> comparer = null)
        {
            var builder = ImmutableDictionary.CreateBuilder<T[], Action<InstructionModificationParser<T>>>(comparer ?? new SequenceComparer());
            builder.AddRange(actions);
            _actions = builder.ToImmutable();
        }

        public bool ParseInstructionModification(IEnumerator<T> enumerator)
        {
            if (_finalized)
            {
                return false;
            }

            List<T> values = new List<T>();
            while (enumerator.MoveNext())
            {
                values.Add(enumerator.Current);
                if (_actions.TryGetValue(values.ToArray(), out var action))
                {
                    action(this);
                    return true;
                }
            }

            if (values.Any())
            {
                throw new Exception($"Reached end of stream looking for instruction");
            }

            return false;
        }

        public void MarkFinalized()
        {
            _finalized = true;
        }
    }
}
