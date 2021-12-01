using System.Collections.Generic;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK.Extensions;
using Newtonsoft.Json.Linq;
using Warehouse.Core;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK
{
    public class EbSoftGoodStorages : IEntities<IStorage>
    {
        private readonly IWebRequest _server;
        private readonly IFilter _filter;

        public EbSoftGoodStorages(IWebRequest server, IGood good)
            : this(server, new StorageGoodsFilter(good))
        {
        }

        public EbSoftGoodStorages(IWebRequest server, IFilter filter)
        {
            _server = server;
            _filter = filter;
        }

        public async Task<IList<IStorage>> ToListAsync()
        {
            var goods = await _server
                .WithFilter(_filter)
                .ReadAsync<List<JObject>>();
            var storages = new List<IStorage>();
            foreach (var good in goods)
            {
                foreach (var storage in good.Value<List<JObject>>("locations"))
                {
                    storages.Add(new EbSoftStorage(_server, storage));
                }
            }
            return storages;
        }

        public IEntities<IStorage> With(IFilter filter)
        {
            return new EbSoftGoodStorages(_server, filter);
        }
    }
}
