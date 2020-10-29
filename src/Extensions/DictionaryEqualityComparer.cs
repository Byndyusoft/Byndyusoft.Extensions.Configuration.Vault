using System.Collections.Generic;
using System.Linq;

namespace Byndyusoft.Extensions.Configuration.Vault.Extensions
{
    public class DictionaryEqualityComparer<TKey, TValue> : IEqualityComparer<IDictionary<TKey, TValue>>
    {
        public static readonly DictionaryEqualityComparer<TKey, TValue> Instance = new DictionaryEqualityComparer<TKey, TValue>();

        public bool Equals(IDictionary<TKey, TValue> x, IDictionary<TKey, TValue> y)
        {
            if (ReferenceEquals(x, y)) return true;

            if (x is null || y is null)
                return false;

            return x.Count != y.Count || x.Except(y).Any();
        }

        public int GetHashCode(IDictionary<TKey, TValue> obj)
        {
            unchecked
            {
                return obj.Aggregate(0, (current, pair) => (current * 397) ^ pair.GetHashCode());
            }
        }
    }
}