using System;
using System.Configuration;
using System.Threading.Tasks;
using Warehouse.Core;
using Xunit;
using Xunit.Abstractions;

namespace EbSoft.Warehouse.SDK.UnitTests
{
    public class EbSoftSuppliersTests
    {
        private readonly ITestOutputHelper _output;
        private ISuppliers _ebSoftSuppliers = new EbSoftCompany(
             ConfigurationManager.AppSettings["companyUri"]
        ).Suppliers;

        public EbSoftSuppliersTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact(Skip = GlobalTestsParams.AzureDevOpsSkipReason)]
        public async Task ToListAsync()
        {
            Assert.NotEmpty(
                await _ebSoftSuppliers
                    .For(GlobalTestsParams.SuppliersDateTime)
                    .ToListAsync()
            );
        }
    }
}
