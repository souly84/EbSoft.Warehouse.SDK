using System.Collections.Generic;
using Warehouse.Core;

namespace EbSoft.Warehouse.SDK
{
    public class ReceptionsGoodsFilter : IFilter
    {
        private readonly string _receptionId;

        public ReceptionsGoodsFilter(string receptionId)
        {
            _receptionId = receptionId;
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
