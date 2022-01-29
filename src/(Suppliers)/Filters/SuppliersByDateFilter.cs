using System;
using System.Collections.Generic;
using Warehouse.Core;

namespace EbSoft.Warehouse.SDK
{
    public class SuppliersByDateFilter : IFilter
    {
        private readonly DateTime _filterDate;

        public SuppliersByDateFilter()
            : this(DateTime.Now)
        {
        }

        public SuppliersByDateFilter(DateTime filterDate)
        {
            _filterDate = filterDate;
        }

        public bool Matches(object entity)
        {
            return entity != null && entity.Equals(_filterDate);
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
