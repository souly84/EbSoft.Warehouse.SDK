using System;
using Warehouse.Core;

namespace EbSoft.Warehouse.SDK
{
    public static class SuppliersExtensions
    {
        public static IEntities<ISupplier> For(this IEntities<ISupplier> suppliers, DateTime dateTime)
        {
            return suppliers.With(new SuppliersByDateFilter(dateTime));
        }
    }
}
