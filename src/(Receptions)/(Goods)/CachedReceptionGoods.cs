using System.Collections.Generic;
using System.Threading.Tasks;
using Warehouse.Core;

namespace EbSoft.Warehouse.SDK
{
    public class CachedReceptionGoods : IReceptionGoods
    {
        private readonly IReceptionGoods _origin;
        private IList<IReceptionGood> _cache;

        public CachedReceptionGoods(IReceptionGoods origin)
        {
            _origin = origin;
        }

        public async Task<IList<IReceptionGood>> ToListAsync()
        {
            if (_cache == null)
            {
                _cache = await _origin.ToListAsync();
            }
            return _cache;
        }

        public IReceptionGood UnkownGood(string barcode)
        {
            return _origin.UnkownGood(barcode);
        }

        public IEntities<IReceptionGood> With(IFilter filter)
        {
            return new CachedReceptionGoods((IReceptionGoods)_origin.With(filter));
        }
    }
}
