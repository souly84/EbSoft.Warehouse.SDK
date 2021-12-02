using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            throw new NotImplementedException();
        }

        public IEntities<IGood> With(IFilter filter)
        {
            return new EbSoftWarehouseGoods(_server, filter);
        }
    }
}
