using System;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Core;

namespace EbSoft.Warehouse.SDK.UnitTests
{
    public static class ReceptionExtensions
    {
        public static async Task<IReception> FirstAsync(this IReceptions receptions)
        {
            var receptionsList = await receptions.ToListAsync();
            return receptionsList.First();
        }

        public static async Task<IReception> FirstAsync(
            this IReceptions receptions,
            Func<IReception, Task<bool>> predicateAsync)
        {
            var receptionsList = await receptions.ToListAsync();
            return await receptionsList.FirstAsync(predicateAsync);
        }

        public static Task<T> FullyConfirmedAsync<T>(this T reception)
            where T : IReception
        {
            return new ConfirmedReception<T>(reception).ConfirmAsync();
        }
    }
}
