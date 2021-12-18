using System;
using System.Linq;
using MediaPrint;
using Newtonsoft.Json.Linq;
using Warehouse.Core;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK.Warehouse
{
    public class EbSoftWarehouseGood : IWarehouseGood
    {
        private readonly IWebRequest _server;
        private readonly JObject _good;
        private IEntities<IStorage> _storages;

        public EbSoftWarehouseGood(IWebRequest server, JObject good)
        {
            _server = server;
            _good = good;
        }

        public int Quantity => new GoodStoragesTotalQuantity(_good).ToInt();

        public IEntities<IStorage> Storages
        {
            get
            {
                if (_storages == null)
                {
                    _storages = new ListOfEntities<IStorage>(
                        _good
                            .Value<JArray>("locations")
                            .Select(storage => new EbSoftStorage(_server, (JObject)storage))
                    );
                }
                return _storages;
            }
        }

        public IMovement Movement => new EbSoftGoodMovement(_server, this);

        public void PrintTo(IMedia media)
        {
            media
                .Put("Data", _good)
                .Put("Storages", Storages)
                .Put("Movement", Movement);
        }
    }
}
