using EbSoft.Warehouse.SDK.Warehouse;
using MediaPrint;
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

        public void PrintTo(IMedia media)
        {
            media.Put("Goods", Goods);
        }
    }
}
