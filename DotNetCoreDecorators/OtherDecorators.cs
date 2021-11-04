using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetCoreDecorators
{
    public static class OtherDecorators
    {

        public static IReadOnlyList<T> AsReadOnlyList<T>(this IEnumerable<T> src)
        {
            if (src is IReadOnlyList<T> readOnlyList)
                return readOnlyList;

            return src.ToList();
        }

        public static IEnumerable<T> ReplaceIfExists<T>(this IEnumerable<T> src, T newElement,
            Func<T, bool> elementToReplaceCallback)
        {
            foreach (var itm in src.Where(itm => !elementToReplaceCallback(itm)))
                yield return itm;

            yield return newElement;
        }
        
    }
}