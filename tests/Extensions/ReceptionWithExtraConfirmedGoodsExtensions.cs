using System.Threading.Tasks;
using Warehouse.Core;

namespace EbSoft.Warehouse.SDK.UnitTests.Extensions
{
    public static class ReceptionWithExtraConfirmedGoodsExtensions
    {
        public static async Task<ReceptionWithExtraConfirmedGoods> ConfirmAsync(
            this ReceptionWithExtraConfirmedGoods reception,
            params string[] barcodes)
        {
            foreach (var barcode in barcodes)
            {
                var good = await reception.ByBarcodeAsync(barcode);
                good.Confirmation.Increase(1);
            }
            return reception;
        }
    }
}
