using Warehouse.Core;

namespace EbSoft.Warehouse.SDK
{
    public static class GoodsExtensions
    {
        public static IEntities<IWarehouseGood> For(this IEntities<IWarehouseGood> goods, string ean)
        {
            return goods.With(new EanGoodsFilter(ean));
        }
    }
}
