using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK.Extensions;
using Warehouse.Core;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK
{
    public class EbSoftReceptionGoods : IReceptionGoods
    {
        private readonly IWebRequest _server;
        private readonly int _receptionId;
        private readonly IFilter _filter;

        public EbSoftReceptionGoods(
            IWebRequest server,
            int receptionId
        ) : this(
                server,
                receptionId,
                new EmptyFilter()
            )
        {
        }

        public EbSoftReceptionGoods(
            IWebRequest server,
            int receptionId,
            IFilter filter)
        {
            _server = server;
            _receptionId = receptionId;
            _filter = filter;
        }

        public async Task<IList<IReceptionGood>> ToListAsync()
        {
            var goods = await _server
                .WithFilter(new ReceptionsGoodsFilter(_receptionId))
                .SelectAsync<IReceptionGood>((good) => new EbSoftReceptionGood(_receptionId, good));

            return goods
                .Where(good => _filter.Matches(good))
                .ToList();
        }

        public IReceptionGood UnkownGood(string barcode)
        {
            return new EbSoftReceptionGood(
                _receptionId,
               barcode
            );
        }

        public IEntities<IReceptionGood> With(IFilter filter)
        {
            return new EbSoftReceptionGoods(
                _server,
                _receptionId,
                filter
            );
        }
    }
}
