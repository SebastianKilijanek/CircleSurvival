using System.Collections.Generic;

public static class IEnumerableExtensions
{
    public static void AddTo<T>(this IEnumerable<T> self, List<T> destination)
    {
        if(self != null) destination.AddRange(self);
    }
}