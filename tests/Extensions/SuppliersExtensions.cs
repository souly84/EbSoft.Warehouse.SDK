using System;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Core;

namespace EbSoft.Warehouse.SDK.UnitTests
{
    public static class SuppliersExtensions
    {
        public static async Task<ISupplier> FirstAsync(this ISuppliers suppliers)
        {
            var suppliersList = await suppliers.ToListAsync();
            return suppliersList.First();
        }

        public static async Task<ISupplier> FirstAsync(
            this ISuppliers suppliers,
            Func<ISupplier, Task<bool>> predicateAsync)
        {
            var suppliersList = await suppliers.ToListAsync();
            return await suppliersList.FirstAsync(predicateAsync);
        }
    }
}
