using System.Collections.Generic;
using System.Linq;
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

        public EbSoftGoodStorages(IWebRequest server, string goodEan)
            : this(server, new EanGoodsFilter(goodEan))
        {
        }

        public EbSoftGoodStorages(IWebRequest server, IFilter filter)
        {
            _server = server;
            _filter = filter;
        }

        public async Task<IList<IStorage>> ToListAsync()
        {
            var good = await _server
                .WithFilter(_filter)
                .ReadAsync<JObject>();
            return good
                .Value<JArray>("locations")
                .Select(storage => new EbSoftStorage(_server, (JObject)storage))
                .ToList<IStorage>();
        }

        public IEntities<IStorage> With(IFilter filter)
        {
            return new EbSoftGoodStorages(_server, filter);
        }
    }
}
