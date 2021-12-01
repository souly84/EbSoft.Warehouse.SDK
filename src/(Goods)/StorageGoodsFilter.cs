using System.Collections.Generic;
using MediaPrint;
using Warehouse.Core;

namespace EbSoft.Warehouse.SDK
{
    public class StorageGoodsFilter : IFilter
    {
        private readonly string _goodEan;

        public StorageGoodsFilter(IGood good)
            : this(good.ToDictionary().Value<string>("Ean"))
        {
        }

        public StorageGoodsFilter(string goodEan)
        {
            _goodEan = goodEan;
        }

        public Dictionary<string, object> ToParams()
        {
            return new Dictionary<string, object>
            {
                { "filter", "getProduct" },
                { "ean", _goodEan },
            };
        }
    }
}
