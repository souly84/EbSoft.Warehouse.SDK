using System.Configuration;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK.UnitTests.Extensions;
using Xunit;
using Assert = EbSoft.Warehouse.SDK.UnitTests.Extensions.Assert;

namespace EbSoft.Warehouse.SDK.UnitTests
{
    public class EbSoftWarehouseTests
    {
        [Fact(Skip = GlobalTestsParams.AzureDevOpsSkipReason)]
        public async Task ByBarcodeInWarehouseIntegration()
        {
            Assert.NotNull(
                await new EbSoftCompany(
                    ConfigurationManager.AppSettings["companyUri"]
                ).Warehouse
                 .ByBarcodeAsync("133037620160")
            );
        }

        [Fact]
        public async Task ByBarcodeInWarehouse()
        {
            Assert.NotNull(
                await new EbSoftCompany(
                    new EbSoftFakeServer().ToWebRequest()
                ).Warehouse
                 .ByBarcodeAsync("133037620160")
            );
        }
    }
}
