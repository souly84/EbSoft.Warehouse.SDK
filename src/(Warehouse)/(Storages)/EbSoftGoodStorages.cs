using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Warehouse.Core;

namespace EbSoft.Warehouse.SDK
{
    public class EbSoftGoodStorages : IStorages
    {
        private readonly JArray _locations;
        private readonly IWarehouseGood _good;
        private IEntities<IStorage> _putAway;
        private IEntities<IStorage> _race;
        private IEntities<IStorage> _reserved;

        public EbSoftGoodStorages(JArray locations, IWarehouseGood good)
        {
            _locations = locations;
            _good = good;
        }

        public IEntities<IStorage> PutAway => _putAway ?? (_putAway = new ListOfEntities<IStorage>(
            _locations.Where(IsPutAway).Select(ToStorage)
        ));

        public IEntities<IStorage> Race => _race ?? (_race = new ListOfEntities<IStorage>(
            _locations.Where(IsRace).Select(ToStorage)
        ));

        public IEntities<IStorage> Reserve => _reserved ?? (_reserved = new ListOfEntities<IStorage>(
            _locations.Where(IsReserve).Select(ToStorage)
        ));

        public Task<IList<IStorage>> ToListAsync()
        {
            return Task.FromResult<IList<IStorage>>(
                _locations
                    .Select(ToStorage)
                    .ToList()
            );
        }

        public IEntities<IStorage> With(IFilter filter)
        {
            throw new NotImplementedException(
                $"Not Clear how to implement this method for {nameof(EbSoftGoodStorages)}"
            );
        }

        private IStorage ToStorage(JToken storage)
        {
            return new EbSoftStorage((JObject)storage, _good);
        }

        private bool IsPutAway(JToken storage)
        {
            return storage.Value<string>("location").Contains("CHECK IN");
        }

        private bool IsRace(JToken storage)
        {
            
            return !IsPutAway(storage) && ValidLocationFormat(storage.Value<string>("location"))[3] == "0";
        }

        private bool IsReserve(JToken storage)
        {
            return !IsPutAway(storage) && !IsRace(storage);
        }

        private string[] ValidLocationFormat(string location)
        {
            var elements = location.Split('.');
            if (elements.Count() < 4)
            {
                throw new InvalidOperationException("Structure of the location is not correct!");
            }
            return elements;
        }
    }
}
