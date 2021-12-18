using System.Collections.Generic;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK.Extensions;
using Newtonsoft.Json.Linq;
using Warehouse.Core;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK.Warehouse
{
    public class EbSoftWarehouseGoods : IEntities<IWarehouseGood>
    {
        private readonly IWebRequest _server;
        private readonly IFilter _filter;

        public EbSoftWarehouseGoods(IWebRequest server, IFilter filter)
        {
            _server = server;
            _filter = filter;
        }

        public async Task<IList<IWarehouseGood>> ToListAsync()
        {
            var good = await _server
               .WithFilter(_filter)
               .ReadAsync<JObject>();
            return new List<IWarehouseGood>
            {
                new EbSoftWarehouseGood(_server, good)
            };
        }

        public IEntities<IWarehouseGood> With(IFilter filter)
        {
            return new EbSoftWarehouseGoods(_server, filter);
        }
    }
}
