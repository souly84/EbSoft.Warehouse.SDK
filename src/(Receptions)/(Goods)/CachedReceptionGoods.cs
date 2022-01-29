using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Core;

namespace EbSoft.Warehouse.SDK
{
    /// <summary>
    /// This entity is pretty tricky one. It's used during reception confirmation process.
    /// Every time when user scans a barcode it tries to find a good using extension method ByBarcodeAsync.
    /// <code>
    /// var goodsByBracode = await confirmation.Reception.Goods.ByBarcodeAsync(barcode);
    /// </code>
    /// As a result it calls <see cref="With(IFilter)"/> method all the time.
    /// To avoid making several requests to the server <see cref="CachedReceptionGoods"/> is used.
    /// </summary>
    public class CachedReceptionGoods : IReceptionGoods
    {
        private readonly IReceptionGoods _origin;
        private readonly IFilter _filter;
        private IList<IReceptionGood> _cache;

        public CachedReceptionGoods(IReceptionGoods origin)
            : this(origin, new EmptyFilter())
        {
        }

        public CachedReceptionGoods(
            IReceptionGoods origin,
            IFilter filter)
        {
            _origin = origin;
            _filter = filter;
        }

        public async Task<IList<IReceptionGood>> ToListAsync()
        {
            if (_cache == null)
            {
                _cache = await _origin.ToListAsync();
                _cache = _cache
                    .Where(good => _filter.Matches(good))
                    .ToList();
            }
            return _cache;
        }

        public IReceptionGood UnkownGood(string barcode)
        {
            return _origin.UnkownGood(barcode);
        }

        /// <summary>
        /// Here there is a tricky moment. The code passes this as dependency to avoid
        /// several calls to the server but should be able to apply the filter to cached result.
        /// </summary>
        public IEntities<IReceptionGood> With(IFilter filter)
        {
            return new CachedReceptionGoods(this, filter);
        }
    }
}
