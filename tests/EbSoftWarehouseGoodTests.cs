using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK.UnitTests.Extensions;
using Warehouse.Core;
using Xunit;
using Assert = EbSoft.Warehouse.SDK.UnitTests.Extensions.Assert;

namespace EbSoft.Warehouse.SDK.UnitTests
{
    public class EbSoftWarehouseGoodTests
    {
        private EbSoftCompany _ebSoftCompany = new EbSoftCompany(
             ConfigurationManager.AppSettings["companyUri"]
        );

        [Fact(Skip = GlobalTestsParams.AzureDevOpsSkipReason)]
        public async Task GoodStoragesIntegration()
        {
            var good = await _ebSoftCompany
                .Warehouse
                .Goods.For("4002516315155")
                .FirstAsync();
            Assert.NotEmpty(
                await good.Storages.ToListAsync()
            );
        }

        [Fact]
        public async Task GoodStorages()
        {
            var good = await new EbSoftCompany(
                new FakeBackend().ToWebRequest()
            ).Warehouse.Goods.For("4002516315155").FirstAsync();
            Assert.Equal(
                2,
                (await good.Storages.ToListAsync()).Count
            );
        }

        [Fact]
        public async Task GoodStoragesTotalQuantity()
        {
            var good = await new EbSoftCompany(
                new FakeBackend().ToWebRequest()
            ).Warehouse.Goods.For("4002516315155").FirstAsync();
            Assert.Equal(
                3,
                good.Quantity
            );
        }

        [Fact(Skip = GlobalTestsParams.AzureDevOpsSkipReason)]
        public async Task StockMovementIntegration()
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

        [Fact]
        public async Task StockMovement()
        {
            var backend = new FakeBackend();
            var good = await new EbSoftCompany(
                backend.ToWebRequest()
            ).Warehouse.Goods.For("4002516315155").FirstAsync();
            var goodStorages = await good.Storages.ToListAsync();
            await good.Movement
                .From(goodStorages.First())
                .MoveToAsync(goodStorages.Last(), 5);
            Assert.EqualJson(
                "",
                backend.Proxy.RequestsContent[1]
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
