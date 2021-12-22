using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK.Extensions;
using Newtonsoft.Json.Linq;
using Warehouse.Core;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK
{
    public class EbSoftGoodStorages : IStorages
    {
        private readonly IWebRequest _server;
        private readonly JArray _locations;
        private IEntities<IStorage> _putAway;
        private IEntities<IStorage> _race;
        private IEntities<IStorage> _reserved;

        public EbSoftGoodStorages(IWebRequest server, JArray locations)
        {
            _server = server;
            _locations = locations;
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

        public async Task<IStorage> ByBarcodeAsync(string ean)
        {
            return new EbSoftStorage(
                await _server.WithQueryParams(
                    new Dictionary<string, string>
                    {
                        { "filter", "getBoxes" },
                        { "ean", ean },
                    }
                ).ReadAsync<JObject>()
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
            return new EbSoftStorage((JObject)storage);
        }

        private bool IsPutAway(JToken storage)
        {
            return storage.Value<string>("location").Contains("CHECK IN");
        }

        private bool IsRace(JToken storage)
        {
            return !IsPutAway(storage) && storage.Value<string>("location").Contains("L-0");
        }

        private bool IsReserve(JToken storage)
        {
            return !IsPutAway(storage) && !IsRace(storage);
        }
    }
}
