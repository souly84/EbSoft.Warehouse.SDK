using System.Collections.Generic;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK.Extensions;
using Warehouse.Core;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK
{
    public class EbSoftReceptionGoods : IEntities<IGood>
    {
        private readonly IWebRequest _server;
        private readonly string _receptionId;
        private readonly IFilter _filter;

        public EbSoftReceptionGoods(
            IWebRequest server,
            string receptionId
        ) : this(
                server,
                receptionId,
                new ReceptionsGoodsFilter(receptionId)
            )
        {
        }

        public EbSoftReceptionGoods(
            IWebRequest server,
            string receptionId,
            IFilter filter)
        {
            _server = server;
            _receptionId = receptionId;
            _filter = filter;
        }

        public Task<IList<IGood>> ToListAsync()
        {
            return _server
                .WithFilter(_filter)
                .SelectAsync<IGood>((good) => new EbSoftReceptionGood(_server, _receptionId, good));
        }

        public IEntities<IGood> With(IFilter filter)
        {
            return new EbSoftReceptionGoods(
                _server,
                _receptionId,
                filter
            );
        }
    }
}
