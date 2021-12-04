using Warehouse.Core;

namespace EbSoft.Warehouse.SDK
{
    public static class GoodsExtensions
    {
        public static IEntities<IGood> For(this IEntities<IGood> goods, string ean)
        {
            return goods.With(new EanGoodsFilter(ean));
        }
    }
}
