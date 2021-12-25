using System;
using System.Configuration;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK.UnitTests.Extensions;
using MediaPrint;
using Warehouse.Core;
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
        public Task ByBarcodeInWarehouse()
        {
            return Assert.EqualJson(
                @"{
                  ""Quantity"": ""0"",
                  ""Location"": ""B.8.0"",
                  ""Number"": ""133037620160""
                }",
                new EbSoftCompany(
                    new FakeBackend().ToWebRequest()
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
