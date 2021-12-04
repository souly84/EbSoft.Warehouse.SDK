using System.Collections.Generic;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK.Extensions;
using Warehouse.Core;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK.Warehouse
{
    public class EbSoftWarehouseGoods : IEntities<IGood>
    {
        private readonly IWebRequest _server;
        private readonly IFilter _filter;

        public EbSoftWarehouseGoods(IWebRequest server, IFilter filter)
        {
            _server = server;
            _filter = filter;
        }

        public Task<IList<IGood>> ToListAsync()
        {
            return _server
               .WithFilter(_filter)
               .SelectAsync<IGood>((good) => new EbSoftWarehouseGood(_server, good));
        }

        public IEntities<IGood> With(IFilter filter)
        {
            return new EbSoftWarehouseGoods(_server, filter);
        }
    }
}
