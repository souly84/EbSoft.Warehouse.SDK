using System.Configuration;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK.UnitTests.Extensions;
using Warehouse.Core;
using Xunit;

namespace EbSoft.Warehouse.SDK.UnitTests
{
    public class EbSoftReceptionTests
    {
        private ISuppliers _ebSoftSuppliers = new EbSoftCompany(
            ConfigurationManager.AppSettings["companyUri"]
        ).Suppliers;

        [Fact(Skip = GlobalTestsParams.AzureDevOpsSkipReason)]
        public async Task ToListAsync()
        {
            var supplier = await _ebSoftSuppliers
                .For(GlobalTestsParams.SuppliersDateTime)
                .FirstAsync();
            Assert.NotEmpty(
                await supplier.Receptions.ToListAsync()
            );
        }

        [Fact(Skip = GlobalTestsParams.AzureDevOpsSkipReason)]
        public async Task ReceptionGoods()
        {
            var reception = await new EbSoftCompanyReception().ReceptionAsync();
            Assert.NotEmpty(
                await reception.Goods.ToListAsync()
            );
        }

        [Fact]
        public async Task Reception_FullConfirmation()
        {
            var reception = await new EbSoftCompanyReception(
                new FakeBackend().ToWebRequest()
            ).ReceptionAsync();

            reception = await reception.FullyConfirmedAsync();
            //Assert.True(
            //     await reception.()
            //);
        }

        //[Fact]
        //public async Task Reception_Confirmation_AddByGood()
        //{
        //    var goodToConfirm = new MockGood("good1", 4);
        //    await new MockReception(
        //        goodToConfirm,
        //        new MockGood("good2", 8)
        //    ).Confirmation().AddAsync(goodToConfirm);
        //    Assert.EqualJson(
        //        @"{
        //            ""Good"":
        //            {
        //                ""Id"": ""good1"",
        //                ""Barcode"": null
        //            },
        //            ""Total"": ""4"",
        //            ""Confirmed"": ""1""
        //        }",
        //        goodToConfirm.Confirmation.ToJson().ToString()
        //    );
        //}

        //[Fact]
        //public async Task Reception_Confirmation_RemoveByGood()
        //{
        //    var goodToConfirm = new MockGood("good1", 4);
        //    var confirmation = new MockReception(
        //        goodToConfirm,
        //        new MockGood("good2", 8)
        //    ).Confirmation();
        //    await confirmation.AddAsync(goodToConfirm);
        //    await confirmation.RemoveAsync(goodToConfirm);
        //    Assert.EqualJson(
        //        @"{
        //            ""Good"":
        //            {
        //                ""Id"": ""good1"",
        //                ""Barcode"": null
        //            },
        //            ""Total"": ""4"",
        //            ""Confirmed"": ""0""
        //        }",
        //        goodToConfirm.Confirmation.ToJson().ToString()
        //    );
        //}

        //[Fact]
        //public async Task Reception_Confirmation_AddByGoodBarcode()
        //{
        //    var goodToConfirm = new MockGood("good1", 4, "360600");
        //    await new MockReception(
        //        goodToConfirm,
        //        new MockGood("good2", 8)
        //    ).Confirmation().AddAsync("360600");
        //    Assert.EqualJson(
        //        @"{
        //            ""Good"":
        //            {
        //                ""Id"": ""good1"",
        //                ""Barcode"": ""360600""
        //            },
        //            ""Total"": ""4"",
        //            ""Confirmed"": ""1""
        //        }",
        //        goodToConfirm.Confirmation.ToJson().ToString()
        //    );
        //}

        //[Fact]
        //public async Task Reception_Confirmation_RemoveByGoodBarcode()
        //{
        //    var goodToConfirm = new MockGood("good1", 4, "360600");
        //    var confirmation = new MockReception(
        //        goodToConfirm,
        //        new MockGood("good2", 8)
        //    ).Confirmation();
        //    await confirmation.AddAsync(goodToConfirm);
        //    await confirmation.RemoveAsync("360600");
        //    Assert.EqualJson(
        //        @"{
        //            ""Good"":
        //            {
        //                ""Id"": ""good1"",
        //                ""Barcode"": ""360600""
        //            },
        //            ""Total"": ""4"",
        //            ""Confirmed"": ""0""
        //        }",
        //        goodToConfirm.Confirmation.ToJson().ToString()
        //    );
        //}
    }
}
