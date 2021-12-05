using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Core;
using Xunit;

namespace EbSoft.Warehouse.SDK.UnitTests
{
    public class EbSoftWarehouseGoodTests
    {
        private EbSoftCompany _ebSoftCompany = new EbSoftCompany(
             ConfigurationManager.AppSettings["companyUri"]
        );

        [Fact(Skip = GlobalTestsParams.AzureDevOpsSkipReason)]
        public async Task GoodStorages()
        {
            var good = await _ebSoftCompany
                .Warehouse
                .Goods.For("4002516315155")
                .FirstAsync();
            Assert.NotEmpty(
                await good.Storages.ToListAsync()
            );
        }

        [Fact(Skip = GlobalTestsParams.AzureDevOpsSkipReason)]
        public async Task StockMovement()
        {
            var good = await _ebSoftCompany
                .Warehouse
                .Goods.For("4002516315155")
                .FirstAsync();
            var goodStorages = await good.Storages.ToListAsync();
            await good.Movement
                .From(goodStorages.First())
                .MoveToAsync(goodStorages.Last(), 5);
            Assert.NotEmpty(
                await good.Storages.ToListAsync()
            );
        }


        [Fact(Skip = GlobalTestsParams.AzureDevOpsSkipReason)]
        public async Task PutAway()
        {
            var good = await _ebSoftCompany
                .Warehouse
                .Goods.For("4002516315155")
                .FirstAsync();
            await good.Movement.MoveToAsync(new MockStorage(), 5);
            Assert.NotEmpty(
                await good.Storages.ToListAsync()
            );
        }
    }
}
