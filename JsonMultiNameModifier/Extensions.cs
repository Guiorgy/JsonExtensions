using System;

namespace Guiorgy.JsonExtensions
{
    internal static class Extensions
    {
        public static bool Any<TSource>(this TSource[] source, Predicate<TSource> predicate) => Array.Exists(source, predicate);
    }
}
