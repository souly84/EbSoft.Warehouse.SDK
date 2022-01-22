using System.Threading.Tasks;
using Warehouse.Core;

namespace EbSoft.Warehouse.SDK.UnitTests.Extensions
{
    public static class ConfirmationExtensions
    {
        public static async Task<IConfirmation> AddAsync(this IConfirmation confirmation, params string[] barcodes)
        {
            foreach (var barcode in barcodes)
            {
                await confirmation.AddAsync(barcode);
            }
            return confirmation;
        }
    }
}
