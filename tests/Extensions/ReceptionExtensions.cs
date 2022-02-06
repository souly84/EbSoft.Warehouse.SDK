using System.Linq;
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

        public static async Task<IReception> ConfirmAsync(
            this IReception reception,
            params string[] barcodes)
        {
            foreach (var barcode in barcodes)
            {
                var goods = await reception.ByBarcodeAsync(barcode, true);
                goods.First().Confirmation.Increase(1);
            }
            await reception.Confirmation().CommitAsync();
            return reception;
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
