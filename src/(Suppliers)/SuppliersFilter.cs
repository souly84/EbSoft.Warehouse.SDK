using System;
using System.Collections.Generic;
using Warehouse.Core;

namespace EbSoft.Warehouse.SDK
{
    public class SuppliersFilter : IFilter
    {
        private readonly DateTime _filterDate;

        public SuppliersFilter()
            : this(DateTime.Now)
        {
        }

        public SuppliersFilter(DateTime filterDate)
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
