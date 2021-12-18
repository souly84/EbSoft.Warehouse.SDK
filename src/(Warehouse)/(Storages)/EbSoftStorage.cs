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

        public EbSoftStorage(JObject storage)
        {
            _storage = storage;
        }

        private string Ean => _storage.Value<string>("ean");

        public IEntities<IWarehouseGood> Goods => throw new NotImplementedException();

        public Task DecreaseAsync(IWarehouseGood good, int quantity)
        {
            throw new NotImplementedException();
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

        public void PrintTo(IMedia media)
        {
            media
                .Put("Number", Ean)
                .Put("Quantity", _storage.Value<int>("quantity"));
        }
    }
}
