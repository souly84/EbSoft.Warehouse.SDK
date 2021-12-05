using System.Collections.Generic;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK.Extensions;
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

        public Task<IList<IWarehouseGood>> ToListAsync()
        {
            return _server
               .WithFilter(_filter)
               .SelectAsync<IWarehouseGood>((good) => new EbSoftWarehouseGood(_server, good));
        }

        public IEntities<IWarehouseGood> With(IFilter filter)
        {
            return new EbSoftWarehouseGoods(_server, filter);
        }
    }
}
