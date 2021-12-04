using MediaPrint;
using Newtonsoft.Json.Linq;
using Warehouse.Core;
using Warehouse.Core.Goods;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK.Warehouse
{
    public class EbSoftWarehouseGood : IGood
    {
        private readonly IWebRequest _server;
        private readonly JObject _good;

        public EbSoftWarehouseGood(IWebRequest server, JObject good)
        {
            _server = server;
            _good = good;
        }

        public int Quantity => throw new System.NotImplementedException();

        public IGoodConfirmation Confirmation => throw new System.NotImplementedException();

        public IEntities<IStorage> Storages => throw new System.NotImplementedException();

        public IMovement Movement => throw new System.NotImplementedException();

        public void PrintTo(IMedia media)
        {
            throw new System.NotImplementedException();
        }
    }
}
