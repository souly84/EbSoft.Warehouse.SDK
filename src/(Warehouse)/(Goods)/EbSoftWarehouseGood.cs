﻿using MediaPrint;
using Newtonsoft.Json.Linq;
using Warehouse.Core;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK
{
    public class EbSoftWarehouseGood : IWarehouseGood
    {
        private readonly IWebRequest _server;
        private readonly JObject _good;
        private IStorages _storages;

        public EbSoftWarehouseGood(IWebRequest server, JObject good)
        {
            _server = server;
            _good = good;
        }

        public int Quantity => new GoodStoragesTotalQuantity(_good).ToInt();

        public IStorages Storages
        {
            get
            {
                if (_storages == null)
                {
                    _storages = new EbSoftGoodStorages(
                        _good.Value<JArray>("locations")
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
                .Put("Quantity", Quantity)
                .Put("Storages", Storages)
                .Put("Movement", Movement);
        }
    }
}
