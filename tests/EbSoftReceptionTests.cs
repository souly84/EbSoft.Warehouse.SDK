using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK.UnitTests.Extensions;
using Newtonsoft.Json.Linq;
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
        public async Task ToListAsyncIntegration()
        {
            var supplier = await _ebSoftSuppliers
                .For(GlobalTestsParams.SuppliersDateTime)
                .FirstAsync();
            Assert.NotEmpty(
                await supplier.Receptions.ToListAsync()
            );
        }

        [Fact(Skip = GlobalTestsParams.AzureDevOpsSkipReason)]
        public async Task ReceptionGoodsIntegration()
        {
            var reception = await new EbSoftCompanyReception().ReceptionAsync();
            Assert.NotEmpty(
                await reception.Goods.ToListAsync()
            );
        }

        [Fact]
        public async Task Reception_FullConfirmation()
        {
            var ebSoftServer = new EbSoftFakeServer();
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
            var ebSoftServer = new EbSoftFakeServer();
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
            var ebSoftServer = new EbSoftFakeServer();
            var reception = await new EbSoftCompanyReception(
                ebSoftServer.ToWebRequest()
            ).ReceptionAsync();
            var goods = await reception.Goods.ToListAsync();
            await reception.Confirmation().AddAsync(goods[0]);
            await reception.Confirmation().AddAsync(goods[0]);
            await reception.Confirmation().RemoveAsync(goods[0]);
            await reception.Confirmation().CommitAsync();
            Assert.Equal(
                new FileContent("./Data/ReceptionConfirmationRequestBody.txt")
                    .ToString()
                    .Replace("\"id\": \"23\"", "\"id\": \"30\"")
                    .Replace("4002515996744", "4002516315155")
                    .NoNewLines(),
                ebSoftServer.Proxy.RequestsContent[2].NoNewLines()
            );
        }

        [Fact]
        public async Task Reception_Confirmation_AddByGoodBarcode()
        {
            var ebSoftServer = new EbSoftFakeServer();
            var reception = await new EbSoftCompanyReception(
                ebSoftServer.ToWebRequest()
            ).ReceptionAsync();
            await reception.AddAndConfirmAsync("4002515996744");
            Assert.Equal(
                new FileContent("./Data/ReceptionConfirmationRequestBody.txt").ToString().NoNewLines(),
                ebSoftServer.Proxy.RequestsContent[2].NoNewLines()
            );
        }

        [Fact]
        public async Task Reception_Confirmation_LastScannedEanIsSentToTheServer()
        {
            var ebSoftServer = new EbSoftFakeServer();
            var reception = await new EbSoftCompanyReception(
                ebSoftServer.ToWebRequest()
            ).ReceptionAsync();
            await reception.AddAndConfirmAsync("4002515996744", "4002515996745");
            Assert.Equal(
                new FileContent("./Data/ReceptionConfirmationLastScannedEanRequestBody.txt").ToString().NoNewLines(),
                ebSoftServer.Proxy.RequestsContent[2].NoNewLines()
            );
        }

        [Fact]
        public async Task Reception_Confirmation_SentToTheServer()
        {
            var ebSoftServer = new EbSoftFakeServer();
            var reception = await new EbSoftCompanyReception(
                ebSoftServer.ToWebRequest()
            ).ReceptionAsync();
            await new StatefulReception(
               reception
                   .WithExtraConfirmed()
                   .WithoutInitiallyConfirmed(),
               new KeyValueStorage()
            ).ConfirmAsync(
                "4002516315155",
                "4002516315155",
                "4002516315155",
                "4002515996745",
                "4002515996745",
                "4002515996745",
                "UnknownBarcode",
                "UnknownBarcode2"
            );
            Assert.Equal(
                new FileContent("./Data/ReceptionExtraConfirmationRequestBody.txt").ToString().NoNewLines(),
                ebSoftServer.Proxy.RequestsContent[2].NoNewLines()
            );
        }

        [Fact]
        public async Task FindByGoodsBarcode()
        {
            Assert.Equal(
                new EbSoftReceptionGood(
                    1,
                    JObject.Parse(
                        @"{
                            ""id"": ""45"",
                            ""oa_dossier"": ""OA859840"",
                            ""article"": ""GROHE 31566SD0"",
                            ""qt"": ""1"",
                            ""ean"": [ ""4005176473234"" ],
                            ""qtin"": ""0"",
                            ""error_code"": null,
                            ""commentaire"": null,
                            ""itemType"": ""electro"",
                            ""qtscanned"": ""0""
                      }"
                    )
                ),
                (await new StatefulReception(
                   new EbSoftReception(
                       new WebRequest.Elegant.WebRequest(
                           "http://nonexisting.com",
                           new FkHttpMessageHandler(
                               new FileContent("./Data/GroheReceptions.json").ToString()
                           )
                       ),
                       1
                   ).WithExtraConfirmed()
                    .WithoutInitiallyConfirmed(),
                   new KeyValueStorage()
                ).ByBarcodeAsync("4005176473234")).First()
            );
        }

        [Fact]
        public async Task ByBarcodeEqualToGoodsInCollection()
        {
            var reception = new StatefulReception(
               new EbSoftReception(
                   new WebRequest.Elegant.WebRequest(
                       "http://nonexisting.com",
                       new FkHttpMessageHandler(
                           new FileContent("./Data/GroheReceptions.json").ToString()
                       )
                   ),
                   1
               ).WithExtraConfirmed()
                .WithoutInitiallyConfirmed(),
               new KeyValueStorage()
            );
            Assert.Equal(
                (await reception.Goods.ToListAsync()).First(g => g.Equals("4005176473234")),
                (await reception.ByBarcodeAsync("4005176473234")).First()
            );
        }

        [Fact]
        public async Task Reception_Confirmation_RemoveByGoodBarcode()
        {
            var ebSoftServer = new EbSoftFakeServer();
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

        [Fact]
        public async Task NeedConfirmation_LeavesNotConfirmedGoodsOnly()
        {
            Assert.Equal(
                new List<IGoodConfirmation>
                {
                    new EbSoftReceptionGood(
                        2,
                        JObject.Parse(@"{
                            ""id"": ""23"",
                            ""oa"": ""OA840701"",
                            ""article"": ""MIELE KM6520FR"",
                            ""qt"": ""2"",
                            ""ean"": [ ""4002515996744"", ""4002515996745"" ],
                            ""qtin"": 0,
                            ""error_code"": null,
                            ""commentaire"": null,
                            ""itemType"": ""electro""
                        }")
                    ).Confirmation
                },
                await new EbSoftReception(
                    new WebRequest.Elegant.WebRequest(
                        "http://nonexisting.com",
                        new FkHttpMessageHandler(
                            new FileContent("./Data/MieleReceptionsAutoConfirmed.json").ToString()
                        )
                    ),
                    2
                ).NotConfirmedOnly().ToListAsync()
            );
        }


    }
}
