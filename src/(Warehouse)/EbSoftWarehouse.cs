using System.Collections.Generic;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK.Extensions;
using EbSoft.Warehouse.SDK.Warehouse;
using MediaPrint;
using Newtonsoft.Json.Linq;
using Warehouse.Core;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK
{
    public class EbSoftWarehouse : IWarehouse
    {
        private readonly IWebRequest _server;

        public EbSoftWarehouse(IWebRequest server)
        {
            _server = server;
        }

        public IEntities<IWarehouseGood> Goods => new EbSoftWarehouseGoods(
            _server,
            new NotWarehouseInitializedFilter()
        );

        public async Task<IStorage> ByBarcodeAsync(string ean)
        {
            return new EbSoftStorage(
                await _server.WithQueryParams(
                    new Dictionary<string, string>
                    {
                        { "filter", "getBoxes" },
                        { "ean", ean },
                    }
                ).ReadAsync<JObject>(),
                new NoWarehouseGood()
            );
        }

        public void PrintTo(IMedia media)
        {
            media.Put("Goods", Goods);
        }
    }
}
