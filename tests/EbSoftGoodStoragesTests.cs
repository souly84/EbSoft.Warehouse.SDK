using System;
using System.Configuration;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK.UnitTests.Extensions;
using Warehouse.Core;
using Xunit;
using Assert = EbSoft.Warehouse.SDK.UnitTests.Extensions.Assert;

namespace EbSoft.Warehouse.SDK.UnitTests
{
    public class EbSoftGoodStoragesTests
    {
        [Fact]
        public async Task PutAwayStorages()
        {
            var good = await new EbSoftCompany(
                new FakeBackend().ToWebRequest()
            ).Warehouse.Goods.For("4002516315155")
             .FirstAsync();
            Assert.Equal(
                1,
                (await good.Storages.PutAway.ToListAsync()).Count
            );
        }

        [Fact]
        public async Task RaceStorages()
        {
            var good = await new EbSoftCompany(
                new FakeBackend().ToWebRequest()
            ).Warehouse.Goods.For("4002516315155")
             .FirstAsync();
            Assert.Equal(
                1,
                (await good.Storages.Race.ToListAsync()).Count
            );
        }

        [Fact]
        public async Task ReserveStorages()
        {
            var good = await new EbSoftCompany(
                new FakeBackend().ToWebRequest()
            ).Warehouse.Goods.For("4002516315155")
             .FirstAsync();
            Assert.Equal(
                0,
                (await good.Storages.Reserve.ToListAsync()).Count
            );
        }

        [Fact(Skip = GlobalTestsParams.AzureDevOpsSkipReason)]
        public async Task ByBarcodeInWarehouse()
        {
            Assert.NotNull(
                await new EbSoftCompany(
                    ConfigurationManager.AppSettings["companyUri"]
                ).Warehouse
                 .ByBarcodeAsync("133037620160")

            );
        }

        //[Fact]
        //public Task ByBarcodeNotExistingBarcode()
        //{
        //    return Assert.ThrowsAsync<InvalidOperationException>(
        //        () =>
        //         new MockStorages(
        //            new ListOfEntities<IStorage>(new MockStorage(), new MockStorage()),
        //            new ListOfEntities<IStorage>(new MockStorage(), new MockStorage()),
        //            new ListOfEntities<IStorage>(new MockStorage("4567890")),
        //            new ListOfEntities<IStorage>(new MockStorage(), new MockStorage())
        //        ).InLocalFirst()
        //         .ByBarcodeAsync("543212")
        //    );
        //}
    }
}
