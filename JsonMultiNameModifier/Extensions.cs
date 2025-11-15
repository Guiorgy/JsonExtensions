using System;

namespace Guiorgy.JsonExtensions
{
    internal static class Extensions
    {
        extension<TSource>(TSource[] source)
        {
            internal bool Any(Predicate<TSource> predicate) => Array.Exists(source, predicate);
        }
    }
}
