using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK.UnitTests.Extensions;
using Warehouse.Core;
using WebRequest.Elegant.Extensions;
using WebRequest.Elegant.Fakes;
using Xunit;
using Assert = EbSoft.Warehouse.SDK.UnitTests.Extensions.Assert;

namespace EbSoft.Warehouse.SDK.UnitTests
{
    public class EbSoftReceptionTests
    {
        private IEntities<ISupplier> _ebSoftSuppliers;

        public EbSoftReceptionTests()
        {
            if (GlobalTestsParams.AzureDevOpsSkipReason == null)
            {
                _ebSoftSuppliers = new EbSoftCompany(
                    ConfigurationManager.AppSettings["companyUri"]
                ).Suppliers;
            }
        }

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
            Assert.Equal(
                File.ReadAllText("./Data/MieleConfirmedGoods.txt")
                    .NoNewLines(),
                ebSoftServer.Proxy.RequestsContent[2].NoNewLines()
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
            Assert.Equal(
                File.ReadAllText("./Data/ReceptionConfirmationRequestBodyQuantity2.txt")
                    .NoNewLines(),
                ebSoftServer.Proxy.RequestsContent[2].NoNewLines()
            );
        }

        [Fact(Skip = GlobalTestsParams.AzureDevOpsSkipReason)]
        public async Task Reception_Confirmation_Integration()
        {
            var proxy = new ProxyHttpMessageHandler();
            var supplier = await new EbSoftCompany(
                new WebRequest.Elegant.WebRequest(
                    ConfigurationManager.AppSettings["companyUri"],proxy)
            ).Suppliers.For(GlobalTestsParams.SuppliersDateTime).FirstAsync();

            var reception = await supplier.Receptions.FirstAsync();
            await reception.FullyConfirmedAsync();

            Assert.EqualJson(
                    @"{""code"":""0"",""message"":""Enregistrement"",""data"":""""}",
                 proxy.ResponsesContent[2]
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
            Assert.Equal(
                new FileContent("./Data/ReceptionConfirmationRequestBody.txt").ToString()
                .Replace("\"id\": \"23\"", "\"id\": \"30\"")
                .Replace("4002515996744", "4002516315155")
                .NoNewLines(),
                ebSoftServer.Proxy.RequestsContent[2].NoNewLines()
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
            Assert.Equal(
                new FileContent("./Data/ReceptionConfirmationRequestBody.txt").ToString().NoNewLines(),
                ebSoftServer.Proxy.RequestsContent[2].NoNewLines()
            );
        }

        [Fact]
        public async Task Reception_Confirmation_LastScannedEanIsSentToTheServer()
        {
            var ebSoftServer = new FakeBackend();
            var reception = await new EbSoftCompanyReception(
                ebSoftServer.ToWebRequest()
            ).ReceptionAsync();
            var confirmation = await reception
                .Confirmation()
                .AddAsync("4002515996744", "4002515996745");
            await confirmation.CommitAsync();
            Assert.Equal(
                new FileContent("./Data/ReceptionConfirmationLastScannedEanRequestBody.txt").ToString().NoNewLines(),
                ebSoftServer.Proxy.RequestsContent[2].NoNewLines()
            );
        }

        [Fact]
        public async Task Reception_Confirmation_SentToTheServer()
        {
            var ebSoftServer = new FakeBackend();
            var reception = await new ReceptionWithExtraConfirmedGoods(
                new ReceptionWithUnkownGoods(
                    await new EbSoftCompanyReception(
                        ebSoftServer.ToWebRequest()
                    ).ReceptionAsync()
                )
            ).ConfirmAsync(
                "4002516315155",
                "4002516315155",
                "4002516315155",
                "4002515996745",
                "4002515996745",
                "UnknownBarcode"
            );
            await reception.Confirmation().CommitAsync();
            Assert.Equal(
                new FileContent("./Data/ReceptionExtraConfirmationRequestBody.txt").ToString().NoNewLines(),
                ebSoftServer.Proxy.RequestsContent[2].NoNewLines()
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
            Assert.Equal(
                new FileContent("./Data/EmptyConfirmationRequestBody.txt").ToString().NoNewLines(),
                ebSoftServer.Proxy.RequestsContent[2].NoNewLines()
            );
        }
    }
}
