using System;
using System.Collections;
using System.Collections.Generic;

namespace DotNetCoreDecorators
{
    public static class ArrayExtentions
    {

        public static IEnumerable<IReadOnlyList<T>> SplitToChunks<T>(this IEnumerable<T> src, int chunkSize)
        {
            var result = new List<T>();

            foreach (var itm in src)
            {
                result.Add(itm);

                if (result.Count >= chunkSize)
                {
                    yield return result;
                    result = new List<T>();
                }
            }

            if (result.Count > 0)
                yield return result;

        }


        public static IEnumerable<T> NullAsEmpty<T>(this IEnumerable<T> src)
        {
            return src ?? Array.Empty<T>();
        }
        
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
        
    }
}