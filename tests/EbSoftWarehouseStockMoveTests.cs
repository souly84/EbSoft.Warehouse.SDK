using System;
using System.Configuration;
using System.Threading.Tasks;
using Warehouse.Core;
using Xunit;

namespace EbSoft.Warehouse.SDK.UnitTests
{
    public class EbSoftWarehouseStockMoveTests
    {
        private EbSoftCompany _ebSoftCompany = new EbSoftCompany(
             ConfigurationManager.AppSettings["companyUri"]
        );

        [Fact(Skip = GlobalTestsParams.AzureDevOpsSkipReason)]
        public async Task MoveFromPutAwayToRace()
        {
            var good = await _ebSoftCompany
                .Warehouse
                .Goods.For("4242005215423")
                .FirstAsync();

            var putAwayStorages = await good.Storages.PutAway.ToListAsync();
            var raceStorages = await good.Storages.Race.ToListAsync();
            var reserveLocations = await good.Storages.Reserve.ToListAsync();

            await good.Movement
                .From(await good.Storages.PutAway.FirstAsync())
                .MoveToAsync(
                    await good.Storages.ByBarcodeAsync("4242005051137"),
                    0
            );

            var goodMoved = await _ebSoftCompany
                .Warehouse
                .Goods.For("4242005051137")
                .FirstAsync();

            putAwayStorages = await good.Storages.PutAway.ToListAsync();
            raceStorages = await good.Storages.Race.ToListAsync();
            reserveLocations = await good.Storages.Reserve.ToListAsync();

            Assert.NotEmpty(
                await good.Storages.ToListAsync()
            );
        }
    }
}
