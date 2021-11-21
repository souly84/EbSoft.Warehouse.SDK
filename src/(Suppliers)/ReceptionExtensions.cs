using System;
using Warehouse.Core;

namespace EbSoft.Warehouse.SDK
{
    public static class SuppliersExtensions
    {
        public static ISuppliers For(this ISuppliers suppliers, DateTime dateTime)
        {
            return suppliers.With(new SuppliersFilter(dateTime));
        }
    }
}
