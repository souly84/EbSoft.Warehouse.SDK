using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK.UnitTests.Extensions;
using MediaPrint;
using Newtonsoft.Json.Linq;
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
                .Goods.FirstAsync("4242005051137");

            Assert.NotEmpty(
                await good.Storages.ToListAsync()
            );
        }

        [Fact]
        public async Task GoodStorages()
        {
            var good = await new EbSoftFakeServer()
                .Warehouse()
                .Goods.FirstAsync("4002516315155");
            Assert.Equal(
                2,
                (await good.Storages.ToListAsync()).Count
            );
        }

        [Fact]
        public async Task GoodGetHashCode()
        {
            Assert.NotEqual(
                0,
                (await new EbSoftFakeServer()
                    .Warehouse()
                    .Goods.FirstAsync("4002516315155")
                ).GetHashCode()
            );
        }

        [Fact]
        public async Task TheSameGood()
        {
            var good = await new EbSoftFakeServer()
                    .Warehouse()
                    .Goods.FirstAsync("4002516315155");
            Xunit.Assert.True(
                good.Equals(good)
            );
        }

        [Fact]
        public Task ThrowsInvalidOperationExceptionWhenNoGoodEan()
        {
            return Assert.ThrowsAsync<InvalidOperationException>(
                () => new EbSoftFakeServer()
                    .Warehouse()
                    .Goods.FirstAsync()
            );
        }

        [Fact]
        public async Task GoodStoragesTotalQuantity()
        {
            var good = await new EbSoftFakeServer()
                .Warehouse()
                .Goods.FirstAsync("4002516315155");
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
                .Goods.FirstAsync("4242005051137");
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
            var fakeServer = new EbSoftFakeServer();
            var good = await fakeServer
                .Warehouse()
                .Goods.FirstAsync("4002516315155");
            await good.Movement
                .From(await good.Storages.ByBarcodeAsync("135332235624"))
                .MoveToAsync(await good.Storages.ByBarcodeAsync("122334461809"), 5);
            Assert.Equal(
                File.ReadAllText("./Data/StockMoveResponse.txt")
                    .NoNewLines(),
                fakeServer.Proxy.RequestsContent[1].NoNewLines()
            );
        }

        [Fact]
        public async Task PutAway()
        {
            var fakeServer = new EbSoftFakeServer();
            var good = await fakeServer
                .Warehouse()
                .Goods.FirstAsync("4002516315155");
            await good.Movement
                .From(await good.Storages.Race.FirstAsync())
                .MoveToAsync(await good.Storages.PutAway.FirstAsync(), 5);

            Assert.Equal(
                File.ReadAllText("./Data/PutAwayStockMoveResponse.txt")
                    .NoNewLines(),
                fakeServer.Proxy.RequestsContent[1].NoNewLines()
            );
        }

        [Fact(Skip = GlobalTestsParams.AzureDevOpsSkipReason)]
        public async Task PutAwayIntegration()
        {
            var good = await _ebSoftCompany
                .Warehouse
                .Goods.FirstAsync("4002516315155");
            await good.Movement.MoveToAsync(new MockStorage(), 5);
            Assert.NotEmpty(
                await good.Storages.ToListAsync()
            );
        }

        [Fact(Skip = GlobalTestsParams.AzureDevOpsSkipReason)]
        public async Task GetLocationFromProductIntegration()
        {
            var good = await _ebSoftCompany
               .Warehouse
               .Goods.FirstAsync("4242005051137");

            Assert.EqualJson(
                @"{
                    ""Quantity"": ""2"",
                    ""Location"": ""CHECK IN ELECTRO.CHECK IN ELECTRO.CHECK IN ELECTRO.0"",
                    ""Number"": ""135332235624""
                  }",
                (await good.Storages.FirstAsync())
                    .ToJson()
                    .ToString()
            );
        }

        [Fact(Skip = GlobalTestsParams.AzureDevOpsSkipReason)]
        public async Task MoveFromPutAwayToRaceIntegration()
        {
            var proxy = new ProxyHttpMessageHandler();
            var company = new EbSoftCompany(
                new WebRequest.Elegant.WebRequest(
                    ConfigurationManager.AppSettings["companyUri"],
                    proxy
                )
            );
            var good = await company
                .Warehouse
                .Goods.FirstAsync("8690842264610");

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

        [Fact]
        public void NotEqualWhenDiffLocations()
        {
            var server = new WebRequest.Elegant.WebRequest("http://nonexisting.com");
            Assert.NotEqual(
                new EbSoftWarehouseGood(
                    server,
                    JObject.Parse(@"{""idarticle"":""699205"",""reference"":""ZANUSSI ZVEEW5X1"",""description"":""45CM - MW"",""fournisseur"":""ELECTROLUX BELGIUM"",""locations"":[{""ean"":""133037917662"",""idlocation"":""15740"",""location"":""B.7.0"",""quantity"":""2""},{""ean"":""133037430102"",""idlocation"":""15753"",""location"":""B.20.0"",""quantity"":""1""},{""ean"":""133037116970"",""idlocation"":""15796"",""location"":""B.63.0"",""quantity"":""2""}],""fromean"":""7332543720071""}")
                ),
                new EbSoftWarehouseGood(
                    server,
                    JObject.Parse(@"{""idarticle"":""699205"",""reference"":""ZANUSSI ZVEEW5X1"",""description"":""45CM - MW"",""fournisseur"":""ELECTROLUX BELGIUM"",""locations"":[{""ean"":""135332235624"",""idlocation"":""1"",""location"":""CHECK IN ELECTRO.CHECK IN ELECTRO.0"",""quantity"":""2""},{""ean"":""133037917662"",""idlocation"":""15740"",""location"":""B.7.0"",""quantity"":""2""},{""ean"":""133037430102"",""idlocation"":""15753"",""location"":""B.20.0"",""quantity"":""1""}],""fromean"":""7332543720071""}"))
            );
        }

        [Fact]
        public void EqualWithTheSameLocations()
        {
            var server = new WebRequest.Elegant.WebRequest("http://nonexisting.com");
            Assert.Equal(
                new EbSoftWarehouseGood(
                    server,
                    JObject.Parse(@"{""idarticle"":""699205"",""reference"":""ZANUSSI ZVEEW5X1"",""description"":""45CM - MW"",""fournisseur"":""ELECTROLUX BELGIUM"",""locations"":[{""ean"":""133037917662"",""idlocation"":""15740"",""location"":""B.7.0"",""quantity"":""2""},{""ean"":""133037430102"",""idlocation"":""15753"",""location"":""B.20.0"",""quantity"":""1""},{""ean"":""133037116970"",""idlocation"":""15796"",""location"":""B.63.0"",""quantity"":""2""}],""fromean"":""7332543720071""}")
                ),
                new EbSoftWarehouseGood(
                    server,
                    JObject.Parse(@"{""idarticle"":""699205"",""reference"":""ZANUSSI ZVEEW5X1"",""description"":""45CM - MW"",""fournisseur"":""ELECTROLUX BELGIUM"",""locations"":[{""ean"":""133037917662"",""idlocation"":""15740"",""location"":""B.7.0"",""quantity"":""2""},{""ean"":""133037430102"",""idlocation"":""15753"",""location"":""B.20.0"",""quantity"":""1""},{""ean"":""133037116970"",""idlocation"":""15796"",""location"":""B.63.0"",""quantity"":""2""}],""fromean"":""7332543720071""}")
                )
            );
        }
    }
}
