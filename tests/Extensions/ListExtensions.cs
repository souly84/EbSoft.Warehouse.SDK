using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EbSoft.Warehouse.SDK.UnitTests
{
    public static class ListExtensions
    {
        public static async Task<IList<T>> WhereAsync<T>(this IEnumerable<T> list, Func<T, Task<bool>> predicateAsync)
        {
            var result = new List<T>();
            foreach (var item in list)
            {
                if (await predicateAsync(item))
                {
                    result.Add(item);
                }
            }
            return result;
        }

        public static async Task<T> FirstAsync<T>(this IEnumerable<T> list, Func<T, Task<bool>> predicateAsync)
        {
            foreach (var item in list)
            {
                if (await predicateAsync(item))
                {
                    return item;
                }
            }
            throw new InvalidOperationException("No items found");
        }
    }
}
