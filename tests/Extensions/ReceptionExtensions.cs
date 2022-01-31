using System.Threading.Tasks;
using Warehouse.Core;

namespace EbSoft.Warehouse.SDK.UnitTests
{
    public static class ReceptionExtensions
    {
        public static Task<T> FullyConfirmedAsync<T>(this T reception)
            where T : IReception
        {
            return new ConfirmedReception<T>(reception).ConfirmAsync();
        }

        public static async Task<IReception> AddAndConfirmAsync(this IReception reception, params string[] barcodes)
        {
            var confirmation = reception.Confirmation();
            foreach (var barcode in barcodes)
            {
                await confirmation.AddAsync(barcode);
            }

            await confirmation.CommitAsync();
            return reception;
        }
    }
}
