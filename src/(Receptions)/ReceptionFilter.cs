using System;
using System.Collections.Generic;
using Warehouse.Core;

namespace EbSoft.Warehouse.SDK
{
    public class ReceptionFilter : IFilter
    {
        private readonly DateTime _filterDate;

        public ReceptionFilter()
            : this(DateTime.Now)
        {
        }

        public ReceptionFilter(DateTime filterDate)
        {
            _filterDate = filterDate;
        }

        public Dictionary<string, object> ToParams()
        {
            return new Dictionary<string, object>
            {
                { "filter", "getListCmr" },
                { "date", _filterDate.ToString("yyyy-MM-dd") },
            };
        }
    }
}
