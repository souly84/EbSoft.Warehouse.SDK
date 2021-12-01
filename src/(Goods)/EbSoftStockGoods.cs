using System.Collections.Generic;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK.Extensions;
using Warehouse.Core;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK
{
    public class EbSoftStockGoods : IEntities<IGood>
    {
        private readonly IWebRequest _server;
        private readonly IFilter _filter;

        public EbSoftStockGoods(
            IWebRequest server,
            string receptionId)
            : this(server, new GoodsFilter(receptionId))
        {
        }

        public EbSoftStockGoods(
            IWebRequest server,
            IFilter filter)
        {
            _server = server;
            _filter = filter;
        }

        public Task<IList<IGood>> ToListAsync()
        {
            return _server
                .WithFilter(_filter)
                .SelectAsync<IGood>((good) => new EbSoftGood(_server, good));
        }

        public IEntities<IGood> With(IFilter filter)
        {
            return new EbSoftStockGoods(_server, filter);
        }
    }
}
