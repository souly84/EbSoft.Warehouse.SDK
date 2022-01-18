using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK.UnitTests.Extensions;
using MediaPrint;
using Warehouse.Core;
using WebRequest.Elegant.Extensions;
using WebRequest.Elegant.Fakes;
using Xunit;
using Assert = EbSoft.Warehouse.SDK.UnitTests.Extensions.Assert;

namespace EbSoft.Warehouse.SDK.UnitTests
{
    public class EbSoftWarehouseGoodTests
    {
        private EbSoftCompany _ebSoftCompany;

        public EbSoftWarehouseGoodTests()
        {
            if (GlobalTestsParams.AzureDevOpsSkipReason == null)
            {
                _ebSoftCompany = new EbSoftCompany(
                    ConfigurationManager.AppSettings["companyUri"]
                );
            }
        }

        [Fact(Skip = GlobalTestsParams.AzureDevOpsSkipReason)]
        public async Task GoodStoragesIntegration()
        {
            var good = await _ebSoftCompany
                .Warehouse
                .Goods.For("4242005051137")
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
        public Task ThrowsInvalidOperationExceptionWhenNoGoodEan()
        {
            return Assert.ThrowsAsync<InvalidOperationException>(
                () => new EbSoftCompany(
                    new FakeBackend().ToWebRequest()
                ).Warehouse.Goods.FirstAsync()
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
                .Goods.For("4242005051137")
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
            await good.Movement
                .From(await good.Storages.ByBarcodeAsync("135332235624"))
                .MoveToAsync(await good.Storages.ByBarcodeAsync("122334461809"), 5);
            Assert.Equal(
                File.ReadAllText("./Data/StockMoveResponse.txt")
                    .NoNewLines(),
                backend.Proxy.RequestsContent[1].NoNewLines()
            );
        }

        [Fact]
        public async Task PutAway()
        {
            var backend = new FakeBackend();
            var good = await new EbSoftCompany(
                backend.ToWebRequest()
            ).Warehouse.Goods.For("8690842264610").FirstAsync();
            await good.Movement
                .From(await good.Storages.Race.FirstAsync())
                .MoveToAsync(await good.Storages.PutAway.FirstAsync(), 5);

            Assert.Equal(
                File.ReadAllText("./Data/PutAwayStockMoveResponse.txt")
                    .NoNewLines(),
                backend.Proxy.RequestsContent[1].NoNewLines()
            );
        }

        [Fact(Skip = GlobalTestsParams.AzureDevOpsSkipReason)]
        public async Task PutAwayIntegration()
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

        [Fact(Skip = GlobalTestsParams.AzureDevOpsSkipReason)]
        public async Task GetLocationFromProduct()
        {
            var good = await _ebSoftCompany
               .Warehouse
               .Goods.For("4242005051137")
               .FirstAsync();
            var goodStorage = await good.Storages.FirstAsync();
            
            Assert.EqualJson(@"{
                    ""Quantity"": ""1"",
                    ""Location"": ""CHECK IN ELECTRO.CHECK IN ELECTRO.CHECK IN ELECTRO.0"",
                    ""Number"": ""135332235624""
                    }", goodStorage.ToJson().ToString());
        }

        //(Skip = GlobalTestsParams.AzureDevOpsSkipReason)
        [Fact]
        public async Task MoveFromPutAwayToRace()
        {
            var proxy = new ProxyHttpMessageHandler();
            var company = new EbSoftCompany(
                new WebRequest.Elegant.WebRequest(
                    ConfigurationManager.AppSettings["companyUri"], proxy)
            );
            var good = await company
                .Warehouse
                .Goods.For("8690842264610")
                .FirstAsync();

            await good.Movement
                .From(await good.Storages.PutAway.FirstAsync())
                .MoveToAsync(
                    await good.Storages.ByBarcodeAsync(company.Warehouse, "134046348189"),
                    1
            );

            Assert.EqualJson(
                    @"{""code"":""0"",""message"":""Enregistrement"",""data"":""""}",
                 proxy.ResponsesContent[2]
            );
        }
    }
}
