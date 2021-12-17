using System;
using System.Threading.Tasks;
using MediaPrint;
using Newtonsoft.Json.Linq;
using Warehouse.Core;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK
{
    public class EbSoftStorage : IStorage
    {
        private readonly JObject _storage;

        public EbSoftStorage(
            IWebRequest server,
            JObject storage)
        {
            _storage = storage;
        }

        public IEntities<IWarehouseGood> Goods => throw new NotImplementedException();

        public Task DecreaseAsync(IWarehouseGood good, int quantity)
        {
            throw new NotImplementedException();
        }

        public Task IncreaseAsync(IWarehouseGood good, int quantity)
        {
            throw new NotImplementedException();
        }

        public void PrintTo(IMedia media)
        {
            media
                .Put("Number", _storage.Value<string>("storage"))
                .Put("Quantity", _storage.Value<int>("quantity"));
        }
    }
}
