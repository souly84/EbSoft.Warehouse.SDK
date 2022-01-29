using System.Configuration;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK.UnitTests.Extensions;
using Warehouse.Core;
using Xunit;
using Assert = Xunit.Assert;

namespace EbSoft.Warehouse.SDK.UnitTests
{
    public class EbSoftSuppliersTests
    {
        private IEntities<ISupplier> _ebSoftSuppliers;

        public EbSoftSuppliersTests()
        {
            if (GlobalTestsParams.AzureDevOpsSkipReason == null)
            {
                _ebSoftSuppliers = new EbSoftCompany(
                    ConfigurationManager.AppSettings["companyUri"]
                ).Suppliers;
            }
        }

        [Fact(Skip = GlobalTestsParams.AzureDevOpsSkipReason)]
        public async Task ToListAsyncIntegration()
        {
            Assert.NotEmpty(
                await _ebSoftSuppliers
                    .For(GlobalTestsParams.SuppliersDateTime)
                    .ToListAsync()
            );
        }

        [Fact]
        public async Task ToListAsync()
        {
            var supplier = await new EbSoftFakeServer()
                .Company().Suppliers
                .For(GlobalTestsParams.SuppliersDateTime)
                .FirstAsync();
            Assert.NotEmpty(
                await supplier.Receptions.ToListAsync()
            );
        }
    }
}
