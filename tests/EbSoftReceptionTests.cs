using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK.UnitTests.Extensions;
using Warehouse.Core;
using Warehouse.Core.Receptions;
using Xunit;
using Assert = EbSoft.Warehouse.SDK.UnitTests.Extensions.Assert;

namespace EbSoft.Warehouse.SDK.UnitTests
{
    public class EbSoftReceptionTests
    {
        private IEntities<ISupplier> _ebSoftSuppliers = new EbSoftCompany(
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
            var ebSoftServer = new FakeBackend();
            var reception = await new EbSoftCompanyReception(
                ebSoftServer.ToWebRequest()
            ).ReceptionAsync();

            await reception.FullyConfirmedAsync();
            Assert.EqualJsonArrays(
                File.ReadAllText("./Data/MieleConfirmedGoods.json"),
                ebSoftServer.Proxy.RequestsContent[2]
            );
        }

        [Fact]
        public async Task Reception_Confirmation_AddByGood()
        {
            var ebSoftServer = new FakeBackend();
            var reception = await new EbSoftCompanyReception(
                ebSoftServer.ToWebRequest()
            ).ReceptionAsync();
            var goods = await reception.Goods.ToListAsync();
            await reception.Confirmation().AddAsync(goods[0]);
            await reception.Confirmation().AddAsync(goods[0]);
            await reception.Confirmation().CommitAsync();
            Assert.EqualJsonArrays(
                @"[
                  {
                    ""id"": ""30"",
                    ""qty"": ""2"",
                    ""gtin"": ""4002516315155"",
                    ""error_code"": null
                  }
                ]",
                 ebSoftServer.Proxy.RequestsContent[2]
            );
        }

        [Fact]
        public async Task Reception_Confirmation_RemoveByGood()
        {
            var ebSoftServer = new FakeBackend();
            var reception = await new EbSoftCompanyReception(
                ebSoftServer.ToWebRequest()
            ).ReceptionAsync();
            var goods = await reception.Goods.ToListAsync();
            await reception.Confirmation().AddAsync(goods[0]);
            await reception.Confirmation().AddAsync(goods[0]);
            await reception.Confirmation().RemoveAsync(goods[0]);
            await reception.Confirmation().CommitAsync();
            Assert.EqualJsonArrays(
                @"[
                  {
                    ""id"": ""30"",
                    ""qty"": ""1"",
                    ""gtin"": ""4002516315155"",
                    ""error_code"": null
                  }
                ]",
                 ebSoftServer.Proxy.RequestsContent[2]
            );
        }

        [Fact]
        public async Task Reception_Confirmation_AddByGoodBarcode()
        {
            var ebSoftServer = new FakeBackend();
            var reception = await new EbSoftCompanyReception(
                ebSoftServer.ToWebRequest()
            ).ReceptionAsync();
            var confirmation = reception.Confirmation();
            await confirmation.AddAsync("4002515996744");
            await confirmation.CommitAsync();
            Assert.EqualJsonArrays(
                @"[
                  {
                    ""id"": ""23"",
                    ""qty"": ""1"",
                    ""gtin"": ""4002515996744"",
                    ""error_code"": null
                  }
                ]",
                 ebSoftServer.Proxy.RequestsContent[2]
            );
        }

        [Fact]
        public async Task Reception_Confirmation_RemoveByGoodBarcode()
        {
            var ebSoftServer = new FakeBackend();
            var reception = await new EbSoftCompanyReception(
                ebSoftServer.ToWebRequest()
            ).ReceptionAsync();
            await reception.Confirmation().AddAsync("4002515996744");
            await reception.Confirmation().RemoveAsync("4002515996744");
            await reception.Confirmation().CommitAsync();
            Assert.EqualJsonArrays(
                @"[]",
                 ebSoftServer.Proxy.RequestsContent[2]
            );
        }
    }
}
