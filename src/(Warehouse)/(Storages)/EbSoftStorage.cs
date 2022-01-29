using System;
using System.Threading.Tasks;
using MediaPrint;
using Newtonsoft.Json.Linq;
using Warehouse.Core;

namespace EbSoft.Warehouse.SDK
{
    public class EbSoftStorage : IStorage
    {
        private readonly JObject _storage;
        private readonly IWarehouseGood _good;

        public EbSoftStorage(
            JObject storage,
            IWarehouseGood good)
        {
            _storage = storage;
            _good = good;
        }

        private string Ean => _storage.Value<string>("ean");

        private int GoodInStorageQuantity => _storage.Value<int>("quantity");

        public IEntities<IWarehouseGood> Goods => new ListOfEntities<IWarehouseGood>(_good);

        public Task<int> QuantityForAsync(IWarehouseGood good)
        {
            if (good.Equals(_good))
            {
                return Task.FromResult(GoodInStorageQuantity);
            }
            return Task.FromResult(0);
        }

        public override bool Equals(object obj)
        {
            return object.ReferenceEquals(this, obj)
                || (obj is string ean && Ean == ean)
                || (obj is IStorage storage && storage.ToDictionary().Value<string>("Number") == Ean);
        }

        public override int GetHashCode()
        {
            return Ean.GetHashCode();
        }

        public Task IncreaseAsync(IWarehouseGood good, int quantity)
        {
            throw new NotImplementedException();
        }

        public Task DecreaseAsync(IWarehouseGood good, int quantity)
        {
            throw new NotImplementedException();
        }

        public void PrintTo(IMedia media)
        {
            media
                .Put("Quantity", GoodInStorageQuantity)
                .Put("Location", _storage.Value<string>("location"))
                .Put("Number", Ean);
        }
    }
}
