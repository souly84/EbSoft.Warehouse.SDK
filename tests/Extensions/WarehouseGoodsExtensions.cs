using System.Threading.Tasks;
using Warehouse.Core;

namespace EbSoft.Warehouse.SDK.UnitTests
{
    public static class WarehouseGoodsExtensions
    {
        public static Task<IWarehouseGood> FirstAsync(
            this IEntities<IWarehouseGood> goods,
            string ean)
        {
            return goods.For(ean).FirstAsync();
        }
    }
}
