using System.Configuration;
using System.Threading.Tasks;
using Warehouse.Core;
using Xunit;

namespace EbSoft.Warehouse.SDK.UnitTests
{
    public class EbSoftReceptionsTests
    {
        private IEntities<ISupplier> _ebSoftSuppliers = new EbSoftCompany(
             ConfigurationManager.AppSettings["companyUri"]
        ).Suppliers;

        [Fact(Skip = GlobalTestsParams.AzureDevOpsSkipReason)]
        public async Task ToListAsync()
        {
            var suppliers = await _ebSoftSuppliers
                .For(GlobalTestsParams.SuppliersDateTime)
                .ToListAsync();
            Assert.NotEmpty(
                await suppliers[0].Receptions.ToListAsync()
            );
        }
    }
}
