using System;
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

        public int Quantity => throw new System.NotImplementedException();

        public IEntities<IStorage> Storages
        {
            get
            {
                if (_storages == null)
                {
                    var ean = _good.Value<string>("ean");
                    if (string.IsNullOrEmpty(ean))
                    {
                        throw new InvalidOperationException(
                            $"Good does not contain Ean\n{_good}"
                        );
                    }

                    _storages = new EbSoftGoodStorages(_server, ean);
                }
                return _storages;
            }
        }

        public IMovement Movement => new EbSoftGoodMovement(_server, this);

        public void PrintTo(IMedia media)
        {
            throw new System.NotImplementedException();
        }
    }
}
