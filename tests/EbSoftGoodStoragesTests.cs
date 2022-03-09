using System;
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
            var good = await new EbSoftFakeServer()
                .Warehouse()
                .Goods.FirstAsync("4002516315155");
            Assert.Equal(
                1,
                (await good.Storages.PutAway.ToListAsync()).Count
            );
        }

        [Fact]
        public async Task PutAwayGoodStorageQuantity()
        {
            var good = await new EbSoftFakeServer()
                .Warehouse()
                .Goods.FirstAsync("4002516315155");
            Assert.Equal(
                2,
                await (await good.Storages.PutAway.FirstAsync()).QuantityForAsync(good)
            );
        }

        [Fact]
        public async Task PutAwayGoodStorageQuantityByBarcode()
        {
            var good = await new EbSoftFakeServer()
                .Warehouse()
                .Goods.FirstAsync("4002516315155");
            Assert.Equal(
                2,
                await (await good.Storages.PutAway.FirstAsync()).QuantityForAsync("4002516315155")
            );
        }

        [Fact]
        public async Task PutAwayGoodStorageZeroQuantityByNonExistingBarcode()
        {
            var good = await new EbSoftFakeServer()
                .Warehouse()
                .Goods.FirstAsync("4002516315155");
            Assert.Equal(
                0,
                await (await good.Storages.PutAway.FirstAsync()).QuantityForAsync("NonExistingBarcode")
            );
        }

        [Fact]
        public async Task PutAwayGoodStorageGetHashCode()
        {
            var good = await new EbSoftFakeServer()
                .Warehouse()
                .Goods.FirstAsync("4002516315155");
            Assert.NotEqual(
                0,
                (await good.Storages.PutAway.FirstAsync()).GetHashCode()
            );
        }

        [Fact]
        public async Task RaceStorages()
        {
            var good = await new EbSoftFakeServer()
                .Warehouse()
                .Goods.FirstAsync("4002516315155");
            Assert.Equal(
                1,
                (await good.Storages.Race.ToListAsync()).Count
            );
        }

        [Fact]
        public Task InvalidOperationException_WhenLocationFormatIsIncorrect()
        {
            return Assert.ThrowsAsync<InvalidOperationException>(() =>
                new EbSoftGoodStorages(
                    Newtonsoft.Json.Linq.JArray.Parse(@"[
                        {
                            ""ean"": ""135332235624"",
                            ""idlocation"": ""1"",
                            ""location"": ""CHECK IN ELECTRO.CHECK IN ELECTRO.CHECK IN ELECTRO.L-0"",
                            ""quantity"": ""2""
                        },
                        {
                            ""ean"": ""122334461809"",
                            ""idlocation"": ""4376"",
                            ""location"": ""ZONE 1.B.5"",
                            ""quantity"": ""1""
                        },
                        {
                            ""ean"": ""122334461808"",
                            ""idlocation"": ""4377"",
                            ""location"": ""ZONE 1.B.0.0"",
                            ""quantity"": ""1""
                        }
                    ]"),
                    new MockWarehouseGood("1", 1)
                ).Race.ToListAsync()
            );
        }

        [Fact]
        public async Task ReserveStorages()
        {
            var good = await new EbSoftFakeServer()
                .Warehouse()
                .Goods.FirstAsync("4002516315155");
            Assert.Equal(
                1,
                (await good.Storages.Reserve.ToListAsync()).Count
            );
        }

        [Fact]
        public async Task DoesNotSupportFilters()
        {
            var good = await new EbSoftFakeServer()
                .Warehouse()
                .Goods.FirstAsync("4002516315155");
            Assert.Throws<NotImplementedException>(() =>
                good.Storages.With(new EmptyFilter())
            );
        }
    }
}
