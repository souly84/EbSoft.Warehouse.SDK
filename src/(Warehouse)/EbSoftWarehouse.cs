using EbSoft.Warehouse.SDK.Warehouse;
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

        public IEntities<IGood> Goods => new EbSoftWarehouseGoods(
            _server,
            new NotWarehouseInitializedFilter()
        );
    }
}
