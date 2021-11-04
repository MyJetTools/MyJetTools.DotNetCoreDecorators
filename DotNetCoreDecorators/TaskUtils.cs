using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetCoreDecorators
{
    public static class TaskUtils
    {
        public static ValueTask<T> AsValueTask<T>(this T itm)
        {
            return new ValueTask<T>(itm);
        }

        public static Task<T> AsTask<T>(this T itm)
        {
            return Task.FromResult(itm);
        }

        public static async Task<List<T>> AsListAsync<T>(this IAsyncEnumerable<T> src)
        {
            var result = new List<T>();

            await foreach (var itm in src)
                result.Add(itm);

            return result;
        }
    }
}