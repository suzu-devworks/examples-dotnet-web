using System.Collections.Generic;

namespace Examples.WebApi.Infrastructure.Extensions;

public static class DictionaryExtensions
{
    public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
    {
        if (source.ContainsKey(key))
        {
            return false;
        }

        source.Add(key, value);
        return true;
    }
}
