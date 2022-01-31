using System.Collections.Generic;
using Warehouse.Core;

namespace EbSoft.Warehouse.SDK
{
    public class ReceptionsGoodsFilter : IFilter
    {
        private readonly int _receptionId;

        public ReceptionsGoodsFilter(int receptionId)
        {
            _receptionId = receptionId;
        }

        public bool Matches(object entity)
        {
            return entity != null && entity.Equals(_receptionId);
        }

        public Dictionary<string, object> ToParams()
        {
            return new Dictionary<string, object>
            {
                { "filter", "getCmrlines" },
                { "id", _receptionId },
            };
        }
    }
}
